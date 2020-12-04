using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CatsOrDogs.API.Infrastructure.Options;
using CatsOrDogs.API.Models;
using CatsOrDogs.API.Models.IO;
using Microsoft.ML;

namespace CatsOrDogs.API.Infrastructure.Services
{
    /// <summary>
    /// Сервис, для классификации изображения
    /// </summary>
    public class ClassifyService
    {
        private readonly MLContext _context;
        private readonly ITransformer _trainedModel;
        private readonly ResoursesInfo _resourses;

        /// <summary/>
        public ClassifyService(
            MLContext context,
            ITransformer trainedModel,
            ResoursesInfo resourses)
        {
            _context = context;
            _trainedModel = trainedModel;
            _resourses = resourses;
        }

        /// <summary/>
        public string Classify(Stream stream)
        {
            var imageInfo = SaveFileToTempDirectory(stream);
            var imageData = _context.Data.LoadFromEnumerable(new List<InputImage>
            {
                imageInfo
            });

            var result = ClassifySingleImage(imageData);
            CleanTempDirectory();

            return result.PredictedLabel;
        }

        /// <summary/>
        private InputImage SaveFileToTempDirectory(Stream stream)
        {
            var filename = Path.Combine(_resourses.TempRelativePath, $"{Guid.NewGuid()}.jpg");

            using (var fileStream = File.Create(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return new InputImage()
            {
                Image = File.ReadAllBytes(filename),
                Label = string.Empty,
                LabelAsKey = 0,
                Path = filename
            };
        }

        private void CleanTempDirectory() =>
            Array.ForEach(Directory.GetFiles(_resourses.TempRelativePath), File.Delete);

        /// <summary/>
        private OutputImage ClassifySingleImage(IDataView data)
        {
            var predictionEngine = _context.Model.CreatePredictionEngine<InputImage, OutputImage>(_trainedModel);
            var image = _context.Data.CreateEnumerable<InputImage>(data, true).First();

            var prediction = predictionEngine.Predict(image);

            return prediction;
        }
    }
}