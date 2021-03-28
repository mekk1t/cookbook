using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
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
        public GetIngredientsResponse GetIngredients(
            [FromRoute] int limit = 25,
            [FromRoute] int offset = 0,
            [FromRoute] bool withRelationships = false)
        {
            var ingredients = _ingredientService.GetIngredients(new GetIngredientsQuery(withRelationships, limit, offset));
            return new GetIngredientsResponse(ingredients);
        }

        [HttpGet("{ingredientName}")]
        public IActionResult GetIngredient([FromRoute] string ingredientName)
        {
            var ingredient = _ingredientService.GetIngredients(new GetIngredientsQuery(withRelationships: true)).FirstOrDefault(i => i.Name == ingredientName);
            if (ingredient == null)
                return NotFound();

            return new JsonResult(new GetSingleIngredientResponse(ingredient.Id, ingredient.Name, ingredient.Categories));
        }

        [HttpDelete("{ingredientId}")]
        public IActionResult DeleteIngredient([FromRoute] Guid ingredientId)
        {
            _ingredientService.DeleteIngredient(new DeleteIngredientCommand(ingredientId));
            return Ok();
        }
    }
}
