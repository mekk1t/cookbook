using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using KitProjects.MasterChef.WebApplication.Recipes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Route("recipes/{recipeId}/steps/{stepId}/ingredients")]
    public class RecipeStepIngredientController : ControllerBase
    {
        private readonly ICommand<ReplaceStepIngredientCommand> _replaceStepIngredient;
        private readonly ICommand<EditStepIngredientDescriptionCommand> _editStepIngredientDescription;
        private readonly ICommand<DeleteIngredientFromStepCommand> _deleteIngredient;
        private readonly ICommand<AppendIngredientToStepCommand> _appendIngredient;

        public RecipeStepIngredientController(
            ICommand<ReplaceStepIngredientCommand> replaceStepIngredient,
            ICommand<EditStepIngredientDescriptionCommand> editStepIngredientDescription,
            ICommand<DeleteIngredientFromStepCommand> deleteIngredient,
            ICommand<AppendIngredientToStepCommand> appendIngredient)
        {
            _replaceStepIngredient = replaceStepIngredient;
            _editStepIngredientDescription = editStepIngredientDescription;
            _deleteIngredient = deleteIngredient;
            _appendIngredient = appendIngredient;
        }

        /// <summary>
        /// Добавляет ингредиент в шаг рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="stepId">ID шага.</param>
        /// <param name="request">Запрос на добавление ингредиента.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppendIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromBody] AppendIngredientRequest request)
        {
            _appendIngredient.Execute(
                new AppendIngredientToStepCommand(
                    new RecipeStepIds(recipeId, stepId),
                    new Ingredient(request.IngredientName),
                    new AppendIngredientParameters(
                        request.Amount,
                        request.Measure,
                        request.Notes)));

            return Ok();
        }

        /// <summary>
        /// Удаляет ингредиент из шага рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="stepId">ID шага в рецепте.</param>
        /// <param name="ingredientId">ID ингредиента, который будет удален.</param>
        /// <returns></returns>
        [HttpDelete("{ingredientId}")]
        public IActionResult DeleteIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromRoute] Guid ingredientId)
        {
            _deleteIngredient.Execute(new DeleteIngredientFromStepCommand(new RecipeStepIds(recipeId, stepId), ingredientId));
            return Ok();
        }
    }
}
