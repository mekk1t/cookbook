using KP.Cookbook.Features.RecipeIngredients.Dtos;
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
        public List<RecipeIngredientDto> Ingredients { get; set; } = new List<RecipeIngredientDto>(0);
    }
}
