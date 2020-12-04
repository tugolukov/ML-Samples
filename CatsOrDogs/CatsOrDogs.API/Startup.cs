using System;
using System.IO;
using CatsOrDogs.API.Infrastructure.Engine;
using CatsOrDogs.API.Infrastructure.Options;
using CatsOrDogs.API.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ML;

namespace CatsOrDogs.API
{
    /// <summary/>
    public class Startup
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly IConfiguration _configuration;

        /// <summary/>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary/>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerDocument(config =>
            {
                config.GenerateExamples = true;
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Simple ML API";
                    document.Info.Description = "Cat or dog on your image???";
                };
            });

            // Подготовка программы к запуску
            var solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                                                                  "..",
                                                                  "..",
                                                                  "..",
                                                                  "..",
                                                                  ".."));
            var resoursesInfo = new ResoursesInfo(solutionDirectory);
            var mlContext = new MLContext();
            
            // Обучение модели
            var engine = new BinaryClassifierEngine(resoursesInfo, mlContext);
            var trainedModel = engine.Learning();
            
            services.AddSingleton(resoursesInfo);
            services.AddSingleton(mlContext);
            services.AddSingleton(trainedModel);
            services.AddScoped<ClassifyService>();
        }

        /// <summary/>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(a =>
            {
                a.DocumentTitle = "ML API Docs";
                a.Path = "/help";
            });

            app.UseReDoc(a => a.Path = "/redoc");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}