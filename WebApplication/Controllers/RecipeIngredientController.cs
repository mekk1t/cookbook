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
        private readonly ICommand<AppendRecipeIngredientCommand> _appendIngredient;
        private readonly ICommand<EditRecipeIngredientDescriptionCommand> _editIngredientDescription;
        private readonly ICommand<RemoveRecipeIngredientCommand> _removeIngredient;
        private readonly ICommand<ReplaceIngredientsListCommand> _replaceIngredientsList;
        private readonly ICommand<ReplaceRecipeIngredientCommand> _replaceIngredient;

        public RecipeIngredientController(
            ICommand<AppendRecipeIngredientCommand> appendIngredient,
            ICommand<EditRecipeIngredientDescriptionCommand> editIngredientDescription,
            ICommand<RemoveRecipeIngredientCommand> removeIngredient,
            ICommand<ReplaceIngredientsListCommand> replaceIngredientsList,
            ICommand<ReplaceRecipeIngredientCommand> replaceIngredient)
        {
            _appendIngredient = appendIngredient;
            _editIngredientDescription = editIngredientDescription;
            _removeIngredient = removeIngredient;
            _replaceIngredientsList = replaceIngredientsList;
            _replaceIngredient = replaceIngredient;
        }

        /// <summary>
        /// Добавляет ингредиент в рецепт.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppendIngredient([FromRoute] Guid recipeId, [FromBody] AppendIngredientRequest request)
        {
            _appendIngredient.Execute(
                new AppendRecipeIngredientCommand(
                    recipeId,
                    new Ingredient(Guid.NewGuid(), request.IngredientName),
                    new AppendIngredientParameters(request.Amount, request.Measure, request.Notes)));
            return Ok();
        }

        /// <summary>
        /// Заменяет все ингредиенты в рецепте на новые. Создает в процессе несуществующие.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на замену ингредиентов.</param>
        [HttpPut]
        public IActionResult ReplaceIngredients([FromRoute] Guid recipeId, [FromBody] ReplaceIngredientsRequest request)
        {
            _replaceIngredientsList.Execute(
                new ReplaceIngredientsListCommand(
                    request.NewIngredients
                        .Select(i => new Ingredient(Guid.NewGuid(), i))
                        .ToArray(),
                    recipeId));

            return Ok();
        }

        /// <summary>
        /// Заменяет старый ингредиент на новый. Новый создается, если отсутствует в базе.
        /// </summary>
        /// <param name="recipeId">ID рецепта, в котором находятся ингредиенты.</param>
        /// <param name="ingredientId">ID старого ингредиента, который будет заменен.</param>
        [HttpPut("{ingredientId}")]
        public IActionResult ReplaceIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] ReplaceIngredientRequest request)
        {
            _replaceIngredient.Execute(
                new ReplaceRecipeIngredientCommand(
                    new Ingredient(
                        ingredientId,
                        request.OldIngredientName),
                    new Ingredient(
                        Guid.NewGuid(),
                        request.NewIngredientName),
                    recipeId));

            return Ok();
        }

        /// <summary>
        /// Редактирует описание ингредиента в рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="ingredientId">ID ингредиента для редактирования.</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{ingredientId}/description")]
        public IActionResult EditIngredientDescription(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] EditRecipeIngredientDescriptionRequest request)
        {
            _editIngredientDescription.Execute(
                new EditRecipeIngredientDescriptionCommand(
                    recipeId,
                    ingredientId,
                    request.Amount,
                    request.Measure,
                    request.Notes));

            return Ok();
        }

        /// <summary>
        /// Удаляет ингредиент из рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта, в котором находится искомый ингредиент.</param>
        /// <param name="ingredientId">ID ингредиента на удаление.</param>
        /// <returns></returns>
        [HttpDelete("{ingredientId}")]
        public IActionResult RemoveIngredient([FromRoute] Guid recipeId, [FromRoute] Guid ingredientId)
        {
            _removeIngredient.Execute(new RemoveRecipeIngredientCommand(recipeId, ingredientId));
            return Ok();
        }
    }
}
