using KitProjects.Api.AspNetCore;
using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace KitProjects.Cookbook.Controllers
{
    public class IngredientsController : ApiJsonController
    {
        private readonly ICrud<Ingredient, long> _crud;
        private readonly IRepository<Ingredient, PaginationFilter> _repository;

        public IngredientsController(
            ILogger<IngredientsController> logger,
            ICrud<Ingredient, long> crud,
            IRepository<Ingredient, PaginationFilter> repository) : base(logger)
        {
            _crud = crud;
            _repository = repository;
        }

        /// <summary>
        /// Список ингредиентов.
        /// </summary>
        /// <param name="filter">Фильтры для списка.</param>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<IngredientResponse>), 200)]
        public IActionResult GetIngredients([FromQuery] PaginationFilter filter) =>
            ExecutePaginatedCollectionRequest(() =>
            {
                if (filter == null)
                    filter = new PaginationFilter();

                var ingredients = _repository.GetList(new PaginationFilter
                {
                    LastId = filter.LastId,
                    Limit = filter.Limit + 1
                });

                if (ingredients.Count == 0)
                    return null;

                bool thereAreMoreIngredients = ingredients.Count == filter.Limit + 1;
                if (thereAreMoreIngredients)
                    ingredients.RemoveAt(ingredients.Count - 1);

                return new PaginatedCollection<IngredientResponse>(ingredients.Select(i => new IngredientResponse(i)).ToArray(), thereAreMoreIngredients);
            });

        /// <summary>
        /// Создание нового ингредиента.
        /// </summary>
        /// <param name="update">Запрос на создание ингредиента.</param>
        /// <response code="200">Созданный ингредиент.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiObjectResponse<IngredientResponse>), 200)]
        public IActionResult CreateIngredient([FromBody] UpdateIngredient update)
        {
            if (update == null)
                return ApiError("Пустое тело запроса.");

            return ExecuteObjectRequest(() =>
            {
                var ingredient = new Ingredient()
                {
                    Name = update.Name
                };
                ingredient.Categories.AddRange(update.Categories.Select(c => new Category(c.Id) { Name = c.Name }));

                return new IngredientResponse(_crud.Create(ingredient));
            });
        }

        /// <summary>
        /// Редактирование ингредиента.
        /// </summary>
        /// <param name="update">Запрос на редактирование ингредиента.</param>
        /// <response code="200">Обновлённый ингредиент.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ApiObjectResponse<IngredientResponse>), 200)]
        public IActionResult UpdateIngredient([FromBody] UpdateIngredient update)
        {
            if (update == null)
                return ApiError("Пустое тело запроса.");

            if (update.Id == default)
                return ApiError("Отсутствует ID ингредиента.");

            return ExecuteObjectRequest(() =>
            {
                var ingredient = new Ingredient(update.Id)
                {
                    Name = update.Name
                };
                ingredient.Categories.AddRange(update.Categories.Select(c => new Category(c.Id) { Name = c.Name }));

                return new IngredientResponse(_crud.Update(ingredient));
            });
        }

        /// <summary>
        /// Подробности об ингредиенте.
        /// </summary>
        /// <param name="id">ID ингредиента в числовом формате.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiObjectResponse<IngredientResponse>), 200)]
        public IActionResult GetIngredientDetails([FromRoute] long id)
        {
            if (id == default)
                return ApiError("Не указан ID ингредиента.");

            return ExecuteObjectRequest(() => new IngredientResponse(_crud.Read(id)));
        }

        /// <summary>
        /// Удаление ингредиента по ID.
        /// </summary>
        /// <param name="id">ID ингредиента в числовом формате.</param>
        /// <response code="204">Ингредиент удален.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult DeleteIngredientById([FromRoute] long id)
        {
            if (id == default)
                return ApiError("Не указан ID ингредиента.");

            return ExecuteAction(() =>
            {
                var ingredient = _crud.Read(id);
                _crud.Delete(ingredient);
            });
        }
    }
}
