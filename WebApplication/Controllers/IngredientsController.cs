using KitProjects.Api.AspNetCore;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Requests.Append;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class IngredientsController : ApiJsonController
    {
        private readonly IngredientsCrud _crud;
        private readonly ICommand<AppendExistingCategoryToIngredientCommand> _appendIngredientCategory;
        private readonly ICommand<RemoveIngredientCategoryCommand> _removeIngredientCategory;

        public IngredientsController(
            IngredientsCrud crud,
            ICommand<RemoveIngredientCategoryCommand> removeIngredientCategory,
            ICommand<AppendExistingCategoryToIngredientCommand> appendIngredientCategory,
            ILogger<IngredientsController> logger) : base(logger)
        {
            _crud = crud;
            _appendIngredientCategory = appendIngredientCategory;
            _removeIngredientCategory = removeIngredientCategory;
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
        [ProducesResponseType(typeof(ApiObjectResponse<Ingredient>), 200)]
        public IActionResult GetIngredient([FromRoute] Guid ingredientId) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("ID ингредиента не может быть значением по умолчанию.");

                var ingredient = _crud.Read(ingredientId);
                if (ingredient == null)
                    throw new Exception($"Не удалось найти ингредиент с ID {ingredientId}");

                return new ApiObjectResponse<Ingredient>(ingredient);
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

        [HttpGet("{ingredientId}/categories")]
        public IActionResult GetIngredientCategories([FromRoute] Guid ingredientId) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("Указан пустой ID ингредиента.");

                var ingredient = _crud.Read(ingredientId);

                return new ApiCollectionResponse<Category>(ingredient.Categories ?? new List<Category>());
            });

        [HttpPost("{ingredientId}/categories")]
        public IActionResult AddCategoryToIngredient(
            [FromRoute] Guid ingredientId,
            [FromBody] AppendCategoryToIngredientRequest request) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("Указан пустой ID ингредиента.");

                if (request == null)
                    throw new Exception("Пустое тело запроса.");

                if (request.CategoryName.IsNullOrEmpty())
                    throw new Exception("Указано пустое имя новой категории.");

                _appendIngredientCategory.Execute(new AppendExistingCategoryToIngredientCommand(request.CategoryName, ingredientId));
            });

        [HttpDelete("{ingredientId}/categories/{categoryId}")]
        public IActionResult RemoveCategoryFromIngredient([FromRoute] Guid ingredientId, [FromRoute] Guid categoryId) =>
            ProcessRequest(() =>
            {
                if (ingredientId == default)
                    throw new Exception("Указан пустой ID ингредиента.");

                if (categoryId == default)
                    throw new Exception("Указан пустой ID категории.");

                _removeIngredientCategory.Execute(new RemoveIngredientCategoryCommand(categoryId, ingredientId));
            });
    }
}
