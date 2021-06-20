using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using KitProjects.MasterChef.WebApplication.Recipes;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Route("recipes/{recipeId}/steps/{stepId}/ingredients")]
    public class RecipeStepIngredientController : ControllerBase
    {
        /// <summary>
        /// Редактирует описание ингредиента в шаге.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="stepId">ID шага.</param>
        /// <param name="ingredientId">ID ингредиента.</param>
        [HttpPut("{ingredientId}/description")]
        public IActionResult EditIngredientDescription(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromRoute] Guid ingredientId,
            [FromBody] EditIngredientDescriptionRequest request)
        {
            _editStepIngredientDescription.Execute(
                new EditStepIngredientDescriptionCommand(
                    new RecipeStepIds(recipeId, stepId),
                    ingredientId,
                    request.Amount,
                    request.Measure));

            return Ok();
        }

        /// <summary>
        /// Заменяет один игредиент в шаге на другой. Новый создается, если прежде не существовал.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="stepId">ID шага.</param>
        /// <param name="ingredientId">ID ингредиента.</param>
        [HttpPut("{ingredientId}")]
        public IActionResult ReplaceIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromRoute] Guid ingredientId,
            [FromBody] ReplaceIngredientRequest request)
        {
            _replaceStepIngredient.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(recipeId, stepId),
                    new Ingredient(ingredientId, request.OldIngredientName),
                    new Ingredient(request.NewIngredientName)));

            return Ok();
        }
    }
}
