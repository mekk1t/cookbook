using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryModerator _moderator;

        public CategoryController(CategoryModerator moderator)
        {
            _moderator = moderator;
        }

        [HttpGet("")]
        public GetCategoriesResponse GetCategories()
        {
            var categories = _moderator.GetCategories();
            return new GetCategoriesResponse(categories);
        }

        [HttpGet("{categoryName}")]
        public GetCategoryResponse GetCategory([FromRoute] string categoryName)
        {
            var category = _moderator.GetCategories().FirstOrDefault(c => c.Name == categoryName);
            return new GetCategoryResponse(category.Id, category.Name);
        }

        [HttpPost("")]
        public IActionResult CreateCategory([FromBody] CreateCategoryRequest request)
        {
            _moderator.CreateCategory(new CreateCategoryCommand(request.Name));
            var createdCategory = _moderator.GetCategories().FirstOrDefault(c => c.Name == request.Name);

            if (createdCategory == null)
            {
                return Conflict("Не удалось создать категорию.");
            }

            return Created(Url.Action(nameof(GetCategory), new { categoryName = createdCategory.Name }), null);
        }

        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory([FromRoute] string categoryName)
        {
            _moderator.DeleteCategory(new DeleteCategoryCommand(categoryName));
            return Ok();
        }

        [HttpPut("{categoryId}")]
        public IActionResult EditCategory([FromRoute] Guid categoryId, [FromBody] EditCategoryRequest request)
        {
            _moderator.EditCategory(new EditCategoryCommand(categoryId, request.NewName));
            return Ok();
        }
    }
}
