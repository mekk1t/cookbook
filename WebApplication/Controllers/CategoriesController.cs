using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using KitProjects.MasterChef.WebApplication.Models.Responses.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoriesController : ApiController
    {
        private readonly ICommand<DeleteCategoryCommand> _deleteCategory;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;
        private readonly IQuery<Category, GetCategoryQuery> _getCategory;
        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly ICommand<EditCategoryCommand> _editCategory;

        public CategoriesController(
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
        /// Список категорий.
        /// </summary>
        /// <param name="withRelationships">Включать ли связанные сущности, например, ингредиенты.</param>
        /// <param name="offset">Отступ данных.</param>
        /// <param name="limit">Ограничение выборки элементов.</param>
        [HttpGet]
        public IActionResult GetCategories(
            [FromQuery] bool withRelationships = false,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 25) =>
            ProcessRequest(() =>
            {
                var categories = _getCategories.Execute(new GetCategoriesQuery(withRelationships, limit, offset));
                if (categories == null || !categories.Any())
                    throw new Exception("Не удалось получить список категорий.");

                var result = categories.Select(cat => new CategoryShortResponse(cat.Id, cat.Name)).ToList();

                return new ApiCollectionResponse<CategoryShortResponse>(result);
            });

        /// <summary>
        /// Создание новой категории.
        /// </summary>
        /// <param name="request">Запрос на создание.</param>
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryRequest request) =>
            ProcessRequest(() =>
            {
                if (request == null)
                    throw new Exception("Тело запроса не может быть пустым.");

                if (request.Name.IsNullOrEmpty())
                    throw new Exception("Имя новой категории не может быть пустым.");

                _createCategory.Execute(new CreateCategoryCommand(request.Name));
                var createdCategory = _getCategory.Execute(new GetCategoryQuery(request.Name));

                if (createdCategory == null)
                    throw new Exception($"Не удалось создать категорию под именем {request.Name}");

                return new
                {
                    IsSuccess = true,
                    CategoryName = createdCategory.Name,
                    createdCategory.Id
                };
            });

        /// <summary>
        /// Подробная информация о категории. Без связей.
        /// </summary>
        /// <param name="categoryId">Название категории.</param>
        [HttpGet("{categoryId}")]
        public IActionResult GetCategory([FromRoute] Guid categoryId) =>
            ProcessRequest(() =>
            {
                if (categoryId == default)
                    throw new Exception("ID категории не может быть пустым.");

                var category = _getCategory.Execute(new GetCategoryQuery(categoryId));
                if (category == null)
                    throw new Exception("Запрашиваемая категория не была найдена.");

                return new ApiResponse<CategoryShortResponse>(new CategoryShortResponse(category.Id, category.Name));
            });

        /// <summary>
        /// Редактирует категорию по ID.
        /// </summary>
        /// <param name="categoryId">ID категории в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{categoryId}")]
        public IActionResult EditCategory([FromRoute] Guid categoryId, [FromBody] EditCategoryRequest request) =>
            ProcessRequest(() =>
            {
                if (categoryId == default)
                    throw new Exception("ID категории не может быть пустым.");
                if (request == null)
                    throw new Exception("Тело запроса не может быть пустым.");
                if (request.NewName.IsNullOrEmpty())
                    throw new Exception("Новое имя категории не может быть пустым.");

                _editCategory.Execute(new EditCategoryCommand(categoryId, request.NewName));
            });

        /// <summary>
        /// Удаляет категорию по ID.
        /// </summary>
        /// <param name="categoryId">Название категории.</param>
        [HttpDelete("{categoryId}")]
        public IActionResult DeleteCategory([FromRoute] Guid categoryId) =>
            ProcessRequest(() =>
            {
                if (categoryId == default)
                    throw new Exception("ID категории не может быть пустым.");

                _deleteCategory.Execute(new DeleteCategoryCommand(categoryId));
            });
    }
}
