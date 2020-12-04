using System.IO;
using System.Threading.Tasks;
using CatsOrDogs.API.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatsOrDogs.API.Controllers
{
    /// <summary>
    /// API для общих методов
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ClassifyService _classify;
        
        /// <summary/>
        public CommonController(ClassifyService classify)
        {
            _classify = classify;
        }

        /// <summary>
        /// Анализ изображения
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> AnalizeImage(IFormFile file)
        {
            if (file == null)
            {
                throw new InvalidDataException("Необходимо загрузить изображение");
            }
            
            using var stream = file.OpenReadStream();
            var result = _classify.Classify(stream);
                
            return Task.FromResult(result);
        }
    }
}