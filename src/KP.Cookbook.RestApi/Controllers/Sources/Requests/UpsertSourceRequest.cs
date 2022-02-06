namespace KP.Cookbook.RestApi.Controllers.Sources.Requests
{
    /// <summary>
    /// Запрос на создание/обновление источника рецептов.
    /// </summary>
    public class UpsertSourceRequest
    {
        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Ссылка на источник: ютуб-канал, личный сайт итд.
        /// </summary>
        public string? Link { get; set; }
        /// <summary>
        /// Картинка: логотип, бренд итд.
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// Одобрено ли использование материалов от этого источника.
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
