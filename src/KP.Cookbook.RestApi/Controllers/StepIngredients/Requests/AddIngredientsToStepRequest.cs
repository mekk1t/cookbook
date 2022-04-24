using KP.Cookbook.Features.RecipeIngredients.Dtos;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.StepIngredients.Requests
{
    /// <summary>
    /// Запрос на добавление ингредиентов в шаг рецепта.
    /// </summary>
    public class AddIngredientsToStepRequest
    {
        public long RecipeId { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
    }
}
