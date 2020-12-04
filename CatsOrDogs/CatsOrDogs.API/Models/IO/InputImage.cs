namespace CatsOrDogs.API.Models.IO
{
    /// <summary>
    /// Схема входных данных
    /// </summary>
    public class InputImage
    {
        /// <summary>
        /// Представление изображения. Модель ожидает, что для обучения используются данные изображений этого типа
        /// </summary>
        public byte[] Image { get; set; }
        
        /// <summary>
        /// Численное представление Label
        /// </summary>
        public uint LabelAsKey { get; set; }

        /// <summary>
        /// Полный путь, по которому хранится изображение
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Категория, к которой принадлежит это изображение. Это прогнозируемое значение
        /// </summary>
        public string Label { get; set; }

    }
}