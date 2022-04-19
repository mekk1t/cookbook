using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.RestApi.Controllers.Recipes.Requests
{
    /// <summary>
    /// Запрос на создание рецепта.
    /// </summary>
    public class CreateRecipeRequest
    {
        /// <summary>
        /// Название рецепта.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Тип рецепта.
        /// </summary>
        public RecipeType Type { get; set; }
        /// <summary>
        /// Тип приготовления.
        /// </summary>
        public CookingType CookingType { get; set; }
        /// <summary>
        /// Тип кухни.
        /// </summary>
        public KitchenType Kitchen { get; set; }
        /// <summary>
        /// Тип праздника.
        /// </summary>
        public HolidayType Holiday { get; set; }
        /// <summary>
        /// Логин пользователя-автора рецепта.
        /// </summary>
        public string? UserLogin { get; set; }
    }
}
