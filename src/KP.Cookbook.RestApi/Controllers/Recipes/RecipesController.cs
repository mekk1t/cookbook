using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Recipes.CreateRecipe;
using KP.Cookbook.Features.Recipes.DeleteRecipe;
using KP.Cookbook.Features.Recipes.GetRecipes;
using KP.Cookbook.Features.Recipes.UpdateRecipe;
using KP.Cookbook.RestApi.Controllers.Recipes.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.Recipes
{
    public class RecipesController : CookbookApiJsonController
    {
        private readonly ICommandHandler<CreateRecipeCommand, Recipe> _createRecipeCommandHandler;
        private readonly ICommandHandler<DeleteRecipeCommand> _deleteRecipeCommandHandler;
        private readonly ICommandHandler<UpdateRecipeCommand> _updateRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipesQuery, List<Recipe>> _getRecipesQueryHandler;

        public RecipesController(
            ILogger<ApiJsonController> logger,
            ICommandHandler<CreateRecipeCommand, Recipe> createRecipeCommandHandler,
            ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler,
            ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler,
            IQueryHandler<GetRecipesQuery, List<Recipe>> getRecipesQueryHandler) : base(logger)
        {
            _createRecipeCommandHandler = createRecipeCommandHandler;
            _deleteRecipeCommandHandler = deleteRecipeCommandHandler;
            _updateRecipeCommandHandler = updateRecipeCommandHandler;
            _getRecipesQueryHandler = getRecipesQueryHandler;
        }

        /// <summary>
        /// Список всех рецептов в приложении.
        /// </summary>
        [HttpGet]
        public IActionResult GetRecipes() => ExecuteCollectionRequest(() => _getRecipesQueryHandler.Execute(GetRecipesQuery.Default));

        /// <summary>
        /// Создание нового рецепта.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request) => ExecuteObjectRequest(() =>
        {
            CreateRecipeCommand? command = null;

            if (string.IsNullOrEmpty(request.Title))
                throw new ArgumentException("Не указано название рецепта", nameof(request.Title));

            if (request.Author?.Nickname != null)
                command = CreateRecipeCommand.CreateWithUserNickname(request.Title, request.Type, request.CookingType, request.Kitchen, request.Holiday, request.Author.Nickname);
            else
                command = CreateRecipeCommand.CreateWithUserLogin(request.Title, request.Type, request.CookingType, request.Kitchen, request.Holiday, request.Author.Login);

            return _createRecipeCommandHandler.Execute(command);
        });

        [HttpPatch("{recipeId}")]
        public IActionResult EditRecipe([FromRoute] long recipeId, [FromBody] EditRecipeRequest request) =>
            ExecuteAction(() => _updateRecipeCommandHandler.Execute(new UpdateRecipeCommand(recipeId, new Source()));

        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] long recipeId) =>
            ExecuteAction(() => _deleteRecipeCommandHandler.Execute(new DeleteRecipeCommand(recipeId)));
    }
}
