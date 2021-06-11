using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class IngredientsController : ApiController
    {
        private readonly IngredientsCrud _crud;

        public IngredientsController(IngredientsCrud crud)
        {
            _crud = crud;
        }

        /// <summary>
        /// Список ингредиентов.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<Ingredient>), 200)]
        public IActionResult GetIngredients([FromQuery] PaginationFilter filter) =>
            ProcessRequest(() =>
            {
                if (filter == null)
                    filter = new PaginationFilter();

                var ingredients = _crud.Read(filter.Limit, filter.Offset);
                if (ingredients == null || !ingredients.Any())
                    throw new Exception("Не удалось получить список ингредиентов.");

                return new ApiCollectionResponse<Ingredient>(ingredients);
            });

        /// <summary>
        /// Создание ингредиента.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateIngredient([FromBody] CreateIngredientRequest request) =>
            ProcessRequest(() =>
            {
                if (request.Name.IsNullOrEmpty())
                    throw new Exception("Указано пустое имя нового ингредиента.");

                _crud.Create(request.Name, request.Categories);
            });

        /// <summary>
        /// Подробная информация об ингредиенте.
        /// </summary>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        [HttpGet("{ingredientId}")]
        [ProducesResponseType(typeof(ApiResponse<Ingredient>), 200)]
        public IActionResult GetIngredient([FromRoute] Guid ingredientId) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("ID ингредиента не может быть значением по умолчанию.");

                var ingredient = _crud.Read(ingredientId);
                if (ingredient == null)
                    throw new Exception($"Не удалось найти ингредиент с ID {ingredientId}");

                return new ApiResponse<Ingredient>(ingredient);
            });

        /// <summary>
        /// Редактирование названия ингредиента по ID.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{ingredientId}")]
        public IActionResult EditIngredient([FromRoute] Guid ingredientId, [FromBody] EditIngredientRequest request) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("ID ингредиента не может быть значением по умолчанию");

                if (request == null)
                    throw new Exception("Пустое тело запроса.");

                if (request.NewName.IsNullOrEmpty())
                    throw new Exception("Указано пустое новое имя.");

                _crud.Update(ingredientId, request.NewName);
            });

        /// <summary>
        /// Удаление ингредиента по ID.
        /// </summary>
        /// <param name="ingredientId">Идентификатор ингредиента в формате GUID.</param>
        [HttpDelete("{ingredientId}")]
        public IActionResult DeleteIngredient([FromRoute] Guid ingredientId) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("ID ингредиента не может быть значением по умолчанию.");

                _crud.Delete(ingredientId);
            });
    }
}
