using KitProjects.Api.AspNetCore;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Responses.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoriesController : ApiJsonController
    {
        private readonly CategoryCrud _crud;

        public CategoriesController(CategoryCrud crud, ILogger<CategoriesController> logger) : base(logger)
        {
            _crud = crud;
        }

        /// <summary>
        /// Список категорий.
        /// </summary>
        /// <remarks>
        /// На данный момент пока без связанных данных.
        /// </remarks>
        /// <param name="filter">Фильтр для списка категорий.</param>
        /// <response code="200">Список категорий.</response>
        /// <response code="500">Ошибка на стороне сервера.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<CategoryShortResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult GetCategories([FromQuery] PaginationFilter filter) =>
            ProcessRequest(() =>
            {
                if (filter == null)
                    filter = new PaginationFilter();

                var categories = _crud.Read(filter.Limit, filter.Offset);
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

                _crud.Create(request.Name);
                var createdCategory = _crud.Read(request.Name);

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

                var category = _crud.Read(categoryId);
                if (category == null)
                    throw new Exception("Запрашиваемая категория не была найдена.");

                return new ApiObjectResponse<CategoryShortResponse>(new CategoryShortResponse(category.Id, category.Name));
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

                _crud.Update(categoryId, request.NewName);
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

                _crud.Delete(categoryId);
            });
    }
}
