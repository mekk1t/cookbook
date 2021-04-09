using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    [Produces("application/json")]
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// Создает рецепт.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost("")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            _recipeService.CreateRecipe(new CreateRecipeCommand(Guid.NewGuid(), request.Title, request.Categories, request.IngredientDetails, request.Steps));
            return Ok();
        }

        /// <summary>
        /// Получает список рецептов. Включает все связи.
        /// </summary>
        [HttpGet("")]
        public IEnumerable<Recipe> GetRecipes() => _recipeService.GetRecipes(new GetRecipesQuery(true));

        /// <summary>
        /// Редактирует рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        public IActionResult EditRecipe(
            [FromRoute] Guid recipeId,
            [FromBody] EditRecipeRequest request,
            [FromServices] ICommand<EditRecipeCommand> command)
        {
            command.Execute(new EditRecipeCommand(recipeId, request.NewTitle, request.NewDescription));
            return Ok();
        }

        /// <summary>
        /// Удаляет рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId, [FromServices] ICommand<DeleteRecipeCommand> command)
        {
            command.Execute(new DeleteRecipeCommand(recipeId));
            return Ok();
        }
    }
}
