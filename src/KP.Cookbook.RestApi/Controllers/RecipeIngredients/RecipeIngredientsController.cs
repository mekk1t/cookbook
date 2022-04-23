using KP.Api.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients
{
    [Route("api/recipes/{recipeId}")]
    public class RecipeIngredientsController : CookbookApiJsonController
    {
        public RecipeIngredientsController(ILogger<ApiJsonController> logger) : base(logger)
        {
        }

        [HttpGet("ingredients")]
        public IActionResult GetRecipeIngredients([FromRoute] long recipeId) => null;

        [HttpPost("ingredients")]
        public IActionResult AddIngredientsToRecipe([FromRoute] long recipeId) => null;

        [HttpPatch("ingredients/{ingredientId}")]
        public IActionResult EditRecipeIngredient([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;

        [HttpDelete("ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] long recipeId, [FromRoute] long ingredientId) => null;
    }
}
