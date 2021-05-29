using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
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
        private readonly IQuery<Category, GetCategoryQuery> _getCategory;
        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly ICommand<EditCategoryCommand> _editCategory;

        public CategoryController(
            ICommand<DeleteCategoryCommand> deleteCategory,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories,
            IQuery<Category, GetCategoryQuery> getCategory,
            ICommand<CreateCategoryCommand> createCategory,
            ICommand<EditCategoryCommand> editCategory)
        {
            _deleteCategory = deleteCategory;
            _getCategories = getCategories;
            _getCategory = getCategory;
            _createCategory = createCategory;
            _editCategory = editCategory;
        }

        /// <summary>
        /// Получает список категорий.
        /// </summary>
        /// <param name="withRelationships">Включать ли связанные сущности, например, ингредиенты.</param>
        /// <param name="offset">Отступ данных.</param>
        /// <param name="limit">Ограничение выборки элементов.</param>
        [HttpGet]
        public GetCategoriesResponse GetCategories(
            [FromQuery] bool withRelationships = false,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 25)
        {
            var categories = _getCategories.Execute(new GetCategoriesQuery(withRelationships, limit, offset));
            return new GetCategoriesResponse(categories);
        }

        /// <summary>
        /// Создает новую категорию в приложении.
        /// </summary>
        /// <param name="request">Запрос на создание.</param>
        [HttpPost]
        public IActionResult CreateCategory(
            [FromBody] CreateCategoryRequest request)
        {
            _createCategory.Execute(new CreateCategoryCommand(request.Name));
            var createdCategory = _getCategory.Execute(new GetCategoryQuery(request.Name));

            if (createdCategory == null)
            {
                return Conflict("Не удалось создать категорию.");
            }

            return Created(Url.Action(nameof(GetCategory), new { categoryName = createdCategory.Name }),
                new { IsSuccess = true, CategoryName = createdCategory.Name, createdCategory.Id });
        }

        /// <summary>
        /// Подробная информация о категории. Без связей.
        /// </summary>
        /// <param name="categoryName">Название категории.</param>
        [HttpGet("{categoryName}")]
        public IActionResult GetCategory(
            [FromRoute] string categoryName)
        {
            var category = _getCategory.Execute(new GetCategoryQuery(categoryName));
            if (category == null)
                return NotFound();

            return Ok(new GetCategoryResponse(category.Id, category.Name));
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
    }
}
