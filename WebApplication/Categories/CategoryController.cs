using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService moderator)
        {
            _categoryService = moderator;
        }

        [HttpGet("")]
        public GetCategoriesResponse GetCategories(
            [FromQuery] bool withRelationships = false,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 25)
        {
            var categories = _categoryService.GetCategories(new GetCategoriesQuery(withRelationships, limit, offset));
            return new GetCategoriesResponse(categories);
        }

        [HttpGet("{categoryName}")]
        public GetCategoryResponse GetCategory([FromRoute] string categoryName)
        {
            var category = _categoryService.GetCategories(new GetCategoriesQuery()).FirstOrDefault(c => c.Name == categoryName);
            return new GetCategoryResponse(category.Id, category.Name);
        }

        [HttpPost("")]
        public IActionResult CreateCategory([FromBody] CreateCategoryRequest request)
        {
            _categoryService.CreateCategory(new CreateCategoryCommand(request.Name));
            var createdCategory = _categoryService.GetCategories(new GetCategoriesQuery()).FirstOrDefault(c => c.Name == request.Name);

            if (createdCategory == null)
            {
                return Conflict("Не удалось создать категорию.");
            }

            return Created(Url.Action(nameof(GetCategory), new { categoryName = createdCategory.Name }), null);
        }

        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory([FromRoute] string categoryName)
        {
            _categoryService.DeleteCategory(new DeleteCategoryCommand(categoryName));
            return Ok();
        }

        [HttpPut("{categoryId}")]
        public IActionResult EditCategory([FromRoute] Guid categoryId, [FromBody] EditCategoryRequest request)
        {
            _categoryService.EditCategory(new EditCategoryCommand(categoryId, request.NewName));
            return Ok();
        }
    }
}
