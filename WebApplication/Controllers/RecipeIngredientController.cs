using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using KitProjects.MasterChef.WebApplication.Recipes;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("recipes/{recipeId}/ingredients")]
    public class RecipeIngredientController : ControllerBase
    {
        /// <summary>
        /// Заменяет все ингредиенты в рецепте на новые. Создает в процессе несуществующие.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на замену ингредиентов.</param>
        [HttpPut]
        public IActionResult ReplaceIngredients([FromRoute] Guid recipeId, [FromBody] ReplaceIngredientsRequest request)
        {
            _replaceIngredientsList.Execute(
                new ReplaceRecipeIngredientsListCommand(
                    request.NewIngredients
                        .Select(i => new Ingredient(Guid.NewGuid(), i))
                        .ToArray(),
                    recipeId));

            return Ok();
        }
    }
}
