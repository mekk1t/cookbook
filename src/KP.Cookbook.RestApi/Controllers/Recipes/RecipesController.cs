using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Recipes.CreateRecipe;
using KP.Cookbook.Features.Recipes.DeleteRecipe;
using KP.Cookbook.Features.Recipes.GetRecipeDetails;
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
        private readonly ICommandHandler<CreateRecipeCommand, long> _createRecipeCommandHandler;
        private readonly ICommandHandler<DeleteRecipeCommand> _deleteRecipeCommandHandler;
        private readonly ICommandHandler<UpdateRecipeCommand> _updateRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipesQuery, List<RecipeDto>> _getRecipesQueryHandler;
        private readonly IQueryHandler<GetRecipeDetailsQuery, RecipeDetailsDto> _getRecipeDetailsQueryHandler;

        public RecipesController(
            ILogger<ApiJsonController> logger,
            ICommandHandler<CreateRecipeCommand, long> createRecipeCommandHandler,
            ICommandHandler<DeleteRecipeCommand> deleteRecipeCommandHandler,
            ICommandHandler<UpdateRecipeCommand> updateRecipeCommandHandler,
            IQueryHandler<GetRecipesQuery, List<RecipeDto>> getRecipesQueryHandler,
            IQueryHandler<GetRecipeDetailsQuery, RecipeDetailsDto> getRecipeDetailsQueryHandler) : base(logger)
        {
            _createRecipeCommandHandler = createRecipeCommandHandler;
            _deleteRecipeCommandHandler = deleteRecipeCommandHandler;
            _updateRecipeCommandHandler = updateRecipeCommandHandler;
            _getRecipesQueryHandler = getRecipesQueryHandler;
            _getRecipeDetailsQueryHandler = getRecipeDetailsQueryHandler;
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
            if (string.IsNullOrEmpty(request.Title))
                throw new ArgumentException("Не указано название рецепта", nameof(request.Title));

            if (string.IsNullOrEmpty(request.UserLogin))
                throw new Exception("Не указан логин пользователя");

            var command = new CreateRecipeCommand(request.Title, request.Type, request.CookingType, request.Kitchen, request.Holiday, request.UserLogin);

            return _createRecipeCommandHandler.Execute(command);
        });

        /// <summary>
        /// Редактирование рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPatch("{recipeId}")]
        public IActionResult EditRecipe([FromRoute] long recipeId, [FromBody] EditRecipeRequest request) =>
            ExecuteAction(() =>
                _updateRecipeCommandHandler.Execute(
                    new UpdateRecipeCommand(
                        recipeId,
                        request.SourceId,
                        request.DurationMinutes ?? 0,
                        request.Description,
                        request.ImageBase64)));

        /// <summary>
        /// Удаление рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] long recipeId) =>
            ExecuteAction(() => _deleteRecipeCommandHandler.Execute(new DeleteRecipeCommand(recipeId)));

        /// <summary>
        /// Подробная информация о рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet("{recipeId}")]
        public IActionResult GetRecipeDetails([FromRoute] long recipeId) =>
            ExecuteObjectRequest(() => _getRecipeDetailsQueryHandler.Execute(new GetRecipeDetailsQuery(recipeId)));
    }
}
