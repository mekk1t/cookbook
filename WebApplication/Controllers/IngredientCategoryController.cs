using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("ingredients/{ingredientId}/categories/{categoryName}")]
    public class IngredientCategoryController : ControllerBase
    {
        private readonly ICommand<AppendIngredientCategoryCommand> _appendIngredientCategory;
        private readonly ICommand<RemoveIngredientCategoryCommand> _removeIngredientCategory;

        public IngredientCategoryController(
            ICommand<RemoveIngredientCategoryCommand> removeIngredientCategory,
            ICommand<AppendIngredientCategoryCommand> appendIngredientCategory)
        {
            _appendIngredientCategory = appendIngredientCategory;
            _removeIngredientCategory = removeIngredientCategory;
        }

        /// <summary>
        /// Добавляет категорию ингредиенту.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="categoryName">Название категории.</param>
        [HttpPut]
        public IActionResult AppendCategory([FromRoute] Guid ingredientId, [FromRoute] string categoryName)
        {
            _appendIngredientCategory.Execute(new AppendIngredientCategoryCommand(categoryName, ingredientId));
            return Ok();
        }

        /// <summary>
        /// Удаляет категорию у ингредиента.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="categoryName">Название категории.</param>
        [HttpDelete]
        public IActionResult RemoveCategory([FromRoute] Guid ingredientId, [FromRoute] string categoryName)
        {
            _removeIngredientCategory.Execute(new RemoveIngredientCategoryCommand(categoryName, ingredientId));
            return Ok();
        }
    }
}
