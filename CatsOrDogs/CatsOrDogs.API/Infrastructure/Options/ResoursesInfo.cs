using System.IO;

namespace CatsOrDogs.API.Infrastructure.Options
{
    /// <summary>
    /// Информация о ресурсах для обучения модели
    /// </summary>
    public class ResoursesInfo
    {
        /// <summary>
        /// Директория проекта
        /// </summary>
        public string SolutionDirectory { get; set; }

        /// <summary>
        /// Директория для сохранения вычисленных значений узких мест
        /// </summary>
        public string WorkspaceRelativePath { get; set; }

        /// <summary>
        /// Директория, где хранится датасет
        /// </summary>
        public string AssetsRelativePath { get; set; }

        /// <summary>
        /// Директория для сохранения временных файлов
        /// </summary>
        public string TempRelativePath { get; set; }

        /// <summary/>
        public ResoursesInfo() { }

        /// <summary/>
        public ResoursesInfo(string solutionDirectory)
        {
            SolutionDirectory = solutionDirectory;
            WorkspaceRelativePath = Path.Combine(solutionDirectory,
                                                 "Resourses",
                                                 "CatsOrDogs",
                                                 "workspace");

            AssetsRelativePath = Path.Combine(solutionDirectory,
                                              "Resourses",
                                              "CatsOrDogs",
                                              "assets");

            TempRelativePath = Path.Combine(solutionDirectory,
                                            "Resourses",
                                            "CatsOrDogs",
                                            "temp");
        }
    }
}