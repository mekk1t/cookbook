using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    [Produces("application/json")]
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly ICommand<CreateRecipeCommand> _createRecipe;
        private readonly IQuery<IEnumerable<Recipe>, GetRecipesQuery> _getRecipes;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _searchRecipe;
        private readonly ICommand<EditRecipeCommand> _editRecipe;
        private readonly ICommand<DeleteRecipeCommand> _deleteRecipe;

        public RecipeController(
            ICommand<CreateRecipeCommand> createRecipe,
            IQuery<IEnumerable<Recipe>, GetRecipesQuery> getRecipes,
            IQuery<RecipeDetails, GetRecipeQuery> searchRecipe,
            ICommand<EditRecipeCommand> editRecipe,
            ICommand<DeleteRecipeCommand> deleteRecipe)
        {
            _createRecipe = createRecipe;
            _getRecipes = getRecipes;
            _searchRecipe = searchRecipe;
            _editRecipe = editRecipe;
            _deleteRecipe = deleteRecipe;
        }

        /// <summary>
        /// Создает рецепт.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            _createRecipe.Execute(new CreateRecipeCommand(Guid.NewGuid(), request.Title, request.Categories, request.IngredientDetails, request.Steps));
            return Ok();
        }

        /// <summary>
        /// Получает список рецептов. Включает все связи.
        /// </summary>
        [HttpGet]
        public IEnumerable<Recipe> GetRecipes() => _getRecipes.Execute(new GetRecipesQuery(true));

        /// <summary>
        /// Получает подробную информацию о рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet("{recipeId}")]
        public RecipeDetails GetRecipe([FromRoute] Guid recipeId) =>
            _searchRecipe.Execute(new GetRecipeQuery(recipeId));

        /// <summary>
        /// Редактирует название и описание рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        public IActionResult EditRecipe(
            [FromRoute] Guid recipeId,
            [FromBody] EditRecipeRequest request)
        {
            _editRecipe.Execute(new EditRecipeCommand(recipeId, request.NewTitle, request.NewDescription));
            return Ok();
        }

        /// <summary>
        /// Удаляет рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId)
        {
            _deleteRecipe.Execute(new DeleteRecipeCommand(recipeId));
            return Ok();
        }
    }
}
