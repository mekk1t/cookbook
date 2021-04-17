using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KitProjects.MasterChef.WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("recipes/{recipeId}/categories/{categoryName}")]
    public class RecipeCategoryController : ControllerBase
    {
        private readonly ICommand<RemoveRecipeCategoryCommand> _removeCategory;
        private readonly ICommand<AppendCategoryToRecipeCommand> _appendCategory;

        public RecipeCategoryController(
            ICommand<RemoveRecipeCategoryCommand> removeCategory,
            ICommand<AppendCategoryToRecipeCommand> appendCategory)
        {
            _removeCategory = removeCategory;
            _appendCategory = appendCategory;
        }

        /// <summary>
        /// Добавляет категорию в рецепт.
        /// </summary>
        /// <param name="recipeId">ID рецепта, куда будет добавлена категория.</param>
        /// <param name="categoryName">Название существующей категории.</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult AppendCategory([FromRoute] Guid recipeId, [FromRoute] string categoryName)
        {
            _appendCategory.Execute(new AppendCategoryToRecipeCommand(categoryName, recipeId));
            return Ok();
        }

        /// <summary>
        /// Удаляет из рецепта категорию по имени.
        /// </summary>
        /// <param name="recipeId">ID рецепта, из которого будет удаляться категория.</param>
        /// <param name="categoryName">Название категории для удаления.</param>
        [HttpDelete]
        public IActionResult RemoveCategory([FromRoute] Guid recipeId, [FromRoute] string categoryName)
        {
            _removeCategory.Execute(new RemoveRecipeCategoryCommand(recipeId, categoryName));
            return Ok();
        }
    }
}
