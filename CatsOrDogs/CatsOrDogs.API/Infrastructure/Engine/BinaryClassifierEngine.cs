using System;
using System.Collections.Generic;
using System.IO;
using CatsOrDogs.API.Infrastructure.Options;
using CatsOrDogs.API.Models;
using Microsoft.ML;
using Microsoft.ML.Vision;

namespace CatsOrDogs.API.Infrastructure.Engine
{
    /// <summary>
    /// Класс, отвечающий за обучение модели 
    /// </summary>
    public class BinaryClassifierEngine
    {
        private readonly ResoursesInfo _resourses;
        private readonly MLContext _context;

        /// <summary/>
        public BinaryClassifierEngine(ResoursesInfo resourses, MLContext context)
        {
            _resourses = resourses;
            _context = context;
        }

        /// <summary>
        /// Обучение модели
        /// </summary>
        /// <returns></returns>
        public ITransformer Learning()
        {
            LogInfo("Чтение данных из директории");
            
            var images = LoadImagesFromDefaultDirectory();
            var imageData = _context.Data.LoadFromEnumerable(images);
            var shuffledData = _context.Data.ShuffleRows(imageData);

            var preprocessingPipeline = _context.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Label",
                               outputColumnName: "LabelAsKey")
                .Append(_context.Transforms.LoadRawImageBytes("Image",
                                                              _resourses.AssetsRelativePath,
                                                              "Path"));
            LogInfo("Подготовка данных к обучению");

            var preProcessedData = preprocessingPipeline
                .Fit(shuffledData)
                .Transform(shuffledData);

            var trainSplit = _context.Data.TrainTestSplit(preProcessedData, 0.3);
            var validationTestSplit = _context.Data.TrainTestSplit(trainSplit.TestSet);

            var trainSet = trainSplit.TrainSet;
            var validationSet = validationTestSplit.TrainSet;

            var classifierOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image",
                LabelColumnName = "LabelAsKey",
                ValidationSet = validationSet,
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                MetricsCallback = Console.WriteLine,
                TestOnTrainSet = false,
                ReuseTrainSetBottleneckCachedValues = true,
                ReuseValidationSetBottleneckCachedValues = true,
                WorkspacePath = _resourses.WorkspaceRelativePath
            };
            
            LogInfo("Обучение модели");

            var trainingPipeline = _context.MulticlassClassification.Trainers
                .ImageClassification(classifierOptions)
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            
            var result = trainingPipeline.Fit(trainSet);
            
            LogInfo("Обучение завершено");

            return result;
        }

        /// <summary>
        /// Загрузка изображения из директории по умолчанию
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ImageInfo> LoadImagesFromDefaultDirectory()
        {
            var files = Directory.GetFiles(_resourses.AssetsRelativePath,
                                           "*",
                                           SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if ((Path.GetExtension(file) != ".jpg") && (Path.GetExtension(file) != ".png"))
                    continue;

                var label = Directory.GetParent(file).Name;

                yield return new ImageInfo()
                {
                    Path = file,
                    Label = label
                };
            }
        }

        private static void LogInfo(string message)
        {
            Console.WriteLine($"{DateTime.Now:T} - {message}");
        }
    }
}