using KP.Cookbook.Features.RecipeIngredients.Dtos;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients.Requests
{
    /// <summary>
    /// Запрос на редактирование ингредиента в рецепте.
    /// </summary>
    public class EditRecipeIngredientRequest
    {
        public RecipeIngredientDto Ingredient { get; set; }
    }
}
