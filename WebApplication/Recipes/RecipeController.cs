using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet("")]
        public IEnumerable<Recipe> GetRecipes() => _recipeService.GetRecipes(new GetRecipesQuery(true));
    }
}
