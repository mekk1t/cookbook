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
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost("")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            _recipeService.CreateRecipe(new CreateRecipeCommand(Guid.NewGuid(), request.Title, request.Categories, request.IngredientDetails, request.Steps));
            return Ok();
        }

        [HttpGet("")]
        public IEnumerable<Recipe> GetRecipes() => _recipeService.GetRecipes(new GetRecipesQuery(true));

        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId, [FromServices] ICommand<DeleteRecipeCommand> command)
        {
            command.Execute(new DeleteRecipeCommand(recipeId));
            return Ok();
        }
    }
}
