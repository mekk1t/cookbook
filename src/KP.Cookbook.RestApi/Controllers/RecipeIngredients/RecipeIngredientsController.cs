using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe;
using KP.Cookbook.RestApi.Controllers.RecipeIngredients.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients
{
    [Route("api/recipes/{recipeId}")]
    public class RecipeIngredientsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<AddIngredientsToRecipeCommand> _addIngredientsToRecipeCommandHandler;

        public RecipeIngredientsController(
            ICommandHandler<AddIngredientsToRecipeCommand> addIngredientsToRecipeCommandHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _addIngredientsToRecipeCommandHandler = addIngredientsToRecipeCommandHandler;
        }

        [HttpGet("ingredients")]
        public IActionResult GetRecipeIngredients([FromRoute] long recipeId) => null;

        [HttpPost("ingredients")]
        public IActionResult AddIngredientsToRecipe([FromRoute] long recipeId, [FromBody] AddIngredientsToRecipeRequest request) =>
            ExecuteAction(() => _addIngredientsToRecipeCommandHandler.Execute(new AddIngredientsToRecipeCommand(recipeId, request.Ingredients)));

        [HttpPatch("ingredients/{ingredientId}")]
        public IActionResult EditRecipeIngredient([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;

        [HttpDelete("ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;
    }
}
