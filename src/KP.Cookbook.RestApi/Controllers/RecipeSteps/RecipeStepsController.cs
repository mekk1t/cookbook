using KP.Api.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KP.Cookbook.RestApi.Controllers.RecipeSteps
{
    /// <summary>
    /// Работа с шагами в рецепте.
    /// </summary>
    [Route("api/recipes/{recipeId}/steps")]
    public class RecipeStepsController : CookbookApiJsonController
    {
        public RecipeStepsController(ILogger<ApiJsonController> logger) : base(logger)
        {

        }

        /// <summary>
        /// Список шагов по приготовлению рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet]
        public IActionResult GetRecipeSteps([FromRoute] long recipeId) => null;

        /// <summary>
        /// Удаление шага из рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpDelete]
        public IActionResult DeleteRecipeStep([FromRoute] long recipeId) => null;

        /// <summary>
        /// Добавление шагов в рецепт.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpPost]
        public IActionResult AddStepsToRecipe([FromRoute] long recipeId) => null;

        /// <summary>
        /// Редактирование данных шага по приготовлению рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpPatch]
        public IActionResult EditRecipeStep([FromRoute] long recipeId) => null;
    }
}
