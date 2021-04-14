using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    [Produces("application/json")]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService moderator)
        {
            _categoryService = moderator;
        }

        /// <summary>
        /// Получает список категорий.
        /// </summary>
        /// <param name="withRelationships">Включать ли связанные сущности, например, ингредиенты.</param>
        /// <param name="offset">Отступ данных.</param>
        /// <param name="limit">Ограничение выборки элементов.</param>
        [HttpGet("")]
        public GetCategoriesResponse GetCategories(
            [FromQuery] bool withRelationships = false,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 25)
        {
            var categories = _categoryService.GetCategories(new GetCategoriesQuery(withRelationships, limit, offset));
            return new GetCategoriesResponse(categories);
        }

        /// <summary>
        /// Подробная информация о категории. Без связей.
        /// </summary>
        /// <param name="categoryName">Название категории.</param>
        [HttpGet("{categoryName}")]
        public IActionResult GetCategory([FromRoute] string categoryName)
        {
            var category = _categoryService.GetCategories(new GetCategoriesQuery()).FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
                return NotFound();

            return Ok(new GetCategoryResponse(category.Id, category.Name));
        }

        /// <summary>
        /// Создает новую категорию в приложении.
        /// </summary>
        /// <param name="request">Запрос на создание.</param>
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

        /// <summary>
        /// Удаляет категорию по названию.
        /// </summary>
        /// <param name="categoryName">Название категории.</param>
        /// <returns></returns>
        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory([FromRoute] string categoryName)
        {
            _categoryService.DeleteCategory(new DeleteCategoryCommand(categoryName));
            return Ok();
        }

        /// <summary>
        /// Редактирует категорию по ID.
        /// </summary>
        /// <param name="categoryId">ID категории в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{categoryId}")]
        public IActionResult EditCategory([FromRoute] Guid categoryId, [FromBody] EditCategoryRequest request)
        {
            _categoryService.EditCategory(new EditCategoryCommand(categoryId, request.NewName));
            return Ok();
        }
    }
}
