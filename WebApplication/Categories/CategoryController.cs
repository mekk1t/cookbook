using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    [Produces("application/json")]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICommand<DeleteCategoryCommand> _deleteCategory;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly CreateCategoryDecorator _categoryService;
        private readonly ICommand<EditCategoryCommand> _editCategory;

        public CategoryController(
            ICommand<DeleteCategoryCommand> deleteCategory,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories,
            IQuery<Category, SearchCategoryQuery> searchCategory,
            CreateCategoryDecorator categoryService,
            ICommand<EditCategoryCommand> editCategory)
        {
            _deleteCategory = deleteCategory;
            _getCategories = getCategories;
            _searchCategory = searchCategory;
            _categoryService = categoryService;
            _editCategory = editCategory;
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
            var categories = _getCategories.Execute(new GetCategoriesQuery(withRelationships, limit, offset));
            return new GetCategoriesResponse(categories);
        }

        /// <summary>
        /// Подробная информация о категории. Без связей.
        /// </summary>
        /// <param name="categoryName">Название категории.</param>
        [HttpGet("{categoryName}")]
        public IActionResult GetCategory(
            [FromRoute] string categoryName)
        {
            var category = _searchCategory.Execute(new SearchCategoryQuery(categoryName));
            if (category == null)
                return NotFound();

            return Ok(new GetCategoryResponse(category.Id, category.Name));
        }

        /// <summary>
        /// Создает новую категорию в приложении.
        /// </summary>
        /// <param name="request">Запрос на создание.</param>
        [HttpPost("")]
        public IActionResult CreateCategory(
            [FromBody] CreateCategoryRequest request)
        {
            _categoryService.CreateCategory(new CreateCategoryCommand(request.Name));
            var createdCategory = _searchCategory.Execute(new SearchCategoryQuery(request.Name));

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
        public IActionResult DeleteCategory(
            [FromRoute] string categoryName)
        {
            _deleteCategory.Execute(new DeleteCategoryCommand(categoryName));
            return Ok();
        }

        /// <summary>
        /// Редактирует категорию по ID.
        /// </summary>
        /// <param name="categoryId">ID категории в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{categoryId}")]
        public IActionResult EditCategory(
            [FromRoute] Guid categoryId,
            [FromBody] EditCategoryRequest request)
        {
            _editCategory.Execute(new EditCategoryCommand(categoryId, request.NewName));
            return Ok();
        }
    }
}
