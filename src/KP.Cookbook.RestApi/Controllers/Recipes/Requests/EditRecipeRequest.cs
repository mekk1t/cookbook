namespace KP.Cookbook.RestApi.Controllers.Recipes.Requests
{
    /// <summary>
    /// Запрос на редактирование рецепта.
    /// </summary>
    public class EditRecipeRequest
    {
        /// <summary>
        /// ID существующего источника рецептов.
        /// </summary>
        public long? SourceId { get; set; }
        /// <summary>
        /// Продолжительность готовки рецепта в минутах.
        /// </summary>
        public int? DurationMinutes { get; set; }
        /// <summary>
        /// Описание рецепта.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Обложка в формате base64.
        /// </summary>
        public string? ImageBase64 { get; set; }
    }
}
