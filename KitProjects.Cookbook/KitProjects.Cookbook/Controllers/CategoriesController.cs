using KitProjects.Api.AspNetCore;
using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace KitProjects.Cookbook.Controllers
{
    public class CategoriesController : ApiJsonController
    {
        private readonly ICrud<Category, long> _crud;
        private readonly IRepository<Category, PaginationFilter> _categoryRepository;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICrud<Category, long> crud,
            IRepository<Category, PaginationFilter> categoryRepository) : base(logger)
        {
            _crud = crud;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Список категорий.
        /// </summary>
        /// <param name="filter">Фильтры для списка.</param>
        /// <response code="200">Список категорий.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<CategoryResponse>), 200)]
        public IActionResult GetCategories([FromQuery] PaginationFilter filter) =>
            ExecutePaginatedCollectionRequest(() =>
            {
                if (filter == null)
                    filter = new PaginationFilter();

                var categories = _categoryRepository.GetList(new PaginationFilter
                {
                    LastId = filter.LastId,
                    Limit = filter.Limit + 1
                });
                if (categories == null)
                    return new PaginatedCollection<CategoryResponse>(null, false);

                bool thereAreMoreCategories = categories.Count == filter.Limit + 1;
                if (thereAreMoreCategories)
                    categories.RemoveAt(categories.Count - 1);

                return new PaginatedCollection<CategoryResponse>(categories.Select(c => new CategoryResponse(c)).ToArray(), thereAreMoreCategories);
            });

        /// <summary>
        /// Подробная информация о категории.
        /// </summary>
        /// <param name="id">ID категории.</param>
        /// <response code="200">Информация о категории.</response>
        /// <response code="404">Категория не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiObjectResponse<CategoryResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetCategoryById([FromRoute] long id) =>
            ExecuteObjectRequest(() => new CategoryResponse(_crud.Read(id)));

        /// <summary>
        /// Создание новой категории.
        /// </summary>
        /// <param name="update">Запрос на создание категории.</param>
        /// <response code="200">Созданная категория.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiObjectResponse<CategoryResponse>), 200)]
        public IActionResult CreateCategory([FromBody] UpdateCategory update)
        {
            if (update == null)
                return ApiError("Пустое тело запроса.");

            return ExecuteObjectRequest(() => new CategoryResponse(_crud.Create(new Category
            {
                Name = update.Name,
                Type = update.Type
            })));
        }

        /// <summary>
        /// Редактирование категории.
        /// </summary>
        /// <param name="update">Запрос на редактирование категории.</param>
        /// <response code="200">Обновленная категория.</response>
        /// <response code="400">Некорректный запрос.</response>
        [ProducesResponseType(typeof(ApiObjectResponse<CategoryResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [HttpPut]
        public IActionResult UpdateCategory([FromBody] UpdateCategory update)
        {
            if (update == null)
                return ApiError("Пустое тело запроса.");

            return ExecuteObjectRequest(() => new CategoryResponse(_crud.Update(new Category(update.Id)
                {
                    Name = update.Name,
                    Type = update.Type
                })));
        }

        /// <summary>
        /// Удаление категории по ID.
        /// </summary>
        /// <param name="id">ID категории.</param>
        /// <response code="204">Категория удалена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteCategory([FromRoute] long id) =>
            ExecuteAction(() =>
            {
                var category = _crud.Read(id);
                _crud.Delete(category);
            });
    }
}
