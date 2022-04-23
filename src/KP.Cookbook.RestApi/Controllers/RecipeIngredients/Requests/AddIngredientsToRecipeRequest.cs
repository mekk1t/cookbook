using KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients.Requests
{
    /// <summary>
    /// Запрос на добавление игредиентов в рецепт.
    /// </summary>
    public class AddIngredientsToRecipeRequest
    {
        /// <summary>
        /// Список новых ингредиентов.
        /// </summary>
        public List<NewRecipeIngredientDto> Ingredients { get; set; } = new List<NewRecipeIngredientDto>(0);
    }
}
