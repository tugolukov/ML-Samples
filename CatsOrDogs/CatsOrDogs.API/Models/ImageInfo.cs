namespace CatsOrDogs.API.Models
{
    /// <summary>
    /// Представление начальных загруженных данных
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// Полный путь, по которому хранится изображение
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// категория, к которой принадлежит это изображение. Это прогнозируемое значение
        /// </summary>
        public string Label { get; set; }
    }
}