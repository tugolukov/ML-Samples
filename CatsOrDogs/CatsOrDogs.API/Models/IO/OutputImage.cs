namespace CatsOrDogs.API.Models.IO
{
    /// <summary>
    /// Схема выходных данных
    /// </summary>
    public class OutputImage
    {
        /// <summary>
        /// Полный путь, по которому хранится изображение
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Исходная категория, к которой принадлежит это изображение. Это прогнозируемое значение
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Значение, спрогнозированное моделью
        /// </summary>
        public string PredictedLabel { get; set; }
    }
}