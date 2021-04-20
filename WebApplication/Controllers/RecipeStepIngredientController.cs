using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Steps;
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
