using KitProjects.MasterChef.Kernel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    [Route("ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientService _ingredientService;
        private readonly CategoryService _categoryService;

        public IngredientController(IngredientService ingredientService, CategoryService categoryService)
        {
            _ingredientService = ingredientService;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public GetIngredientsResponse GetIngredients() => new(_ingredientService.GetIngredients());

        [HttpGet("{ingredientName}")]
        public IActionResult GetIngredient([FromRoute] string ingredientName)
        {
            var ingredient = _ingredientService.GetIngredients().FirstOrDefault(i => i.Name == ingredientName);
            if (ingredient == null)
                return NotFound();

            if (ingredient.Categories?.Count == 0)
            {
                var ingredientCategories =
            }

            return Content(JsonSerializer.Serialize(new GetSingleIngredientResponse(ingredient.Id, ingredient.Name)));
        }
    }
}
