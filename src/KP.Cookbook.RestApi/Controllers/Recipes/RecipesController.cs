using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Recipes.CreateRecipe;
using KP.Cookbook.Features.Recipes.DeleteRecipe;
using KP.Cookbook.Features.Recipes.GetRecipes;
using KP.Cookbook.Features.Recipes.UpdateRecipe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.Recipes
{
    public class RecipesController : CookbookApiJsonController
    {
        private readonly ICommandHandler<CreateRecipeCommand> _createRecipeCommandHandler;
        private readonly ICommandHandler<DeleteRecipeCommand> _deleteRecipeCommandHandler;
        private readonly ICommandHandler<UpdateRecipeCommand> _updateRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipesQuery, List<Recipe>> _getRecipesQueryHandler;

        public RecipesController(
            ILogger<ApiJsonController> logger,
            ICommandHandler<CreateRecipeCommand> createRecipeCommandHandler,
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

        [HttpPost]
        public IActionResult CreateRecipe() => ExecuteObjectRequest(() => _createRecipeCommandHandler.Execute());

        [HttpPatch]
        public IActionResult EditRecipe() => ExecuteAction(() => _updateRecipeCommandHandler.Execute());

        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] long recipeId) =>
            ExecuteAction(() => _deleteRecipeCommandHandler.Execute(new DeleteRecipeCommand(recipeId)));
    }
}
