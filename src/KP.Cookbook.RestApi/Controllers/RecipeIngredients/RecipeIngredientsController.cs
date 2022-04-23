using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe;
using KP.Cookbook.Features.RecipeIngredients.GetRecipeIngredients;
using KP.Cookbook.RestApi.Controllers.RecipeIngredients.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients
{
    [Route("api/recipes/{recipeId}")]
    public class RecipeIngredientsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<AddIngredientsToRecipeCommand> _addIngredientsToRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipeIngredientsQuery, List<IngredientDetailed>> _getRecipeIngredientsQueryHandler;

        public RecipeIngredientsController(
            ICommandHandler<AddIngredientsToRecipeCommand> addIngredientsToRecipeCommandHandler,
            IQueryHandler<GetRecipeIngredientsQuery, List<IngredientDetailed>> getRecipeIngredientsQueryHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _addIngredientsToRecipeCommandHandler = addIngredientsToRecipeCommandHandler;
            _getRecipeIngredientsQueryHandler = getRecipeIngredientsQueryHandler;
        }

        [HttpGet("ingredients")]
        public IActionResult GetRecipeIngredients([FromRoute] long recipeId) =>
            ExecuteCollectionRequest(() => _getRecipeIngredientsQueryHandler.Execute(new GetRecipeIngredientsQuery(recipeId)));

        [HttpPost("ingredients")]
        public IActionResult AddIngredientsToRecipe([FromRoute] long recipeId, [FromBody] AddIngredientsToRecipeRequest request) =>
            ExecuteAction(() => _addIngredientsToRecipeCommandHandler.Execute(new AddIngredientsToRecipeCommand(recipeId, request.Ingredients)));

        [HttpPatch("ingredients/{ingredientId}")]
        public IActionResult EditRecipeIngredient([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;

        [HttpDelete("ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;
    }
}
