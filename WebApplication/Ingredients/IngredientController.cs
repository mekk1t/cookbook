using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    [Produces("application/json")]
    [Route("ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly ICommand<CreateIngredientCommand> _createIngredient;
        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly IngredientEditor _editor;
        private readonly IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> _getIngredients;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly ICommand<DeleteIngredientCommand> _deleteIngredient;
        private readonly ICommand<EditIngredientCommand> _editIngredient;

        public IngredientController(
            ICommand<CreateIngredientCommand> createIngredient,
            ICommand<CreateCategoryCommand> createCategory,
            IngredientEditor editor,
            IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> getIngredients,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            ICommand<DeleteIngredientCommand> deleteIngredient,
            ICommand<EditIngredientCommand> editIngredient)
        {
            _createIngredient = createIngredient;
            _createCategory = createCategory;
            _editor = editor;
            _getIngredients = getIngredients;
            _searchIngredient = searchIngredient;
            _deleteIngredient = deleteIngredient;
            _editIngredient = editIngredient;
        }

        /// <summary>
        /// Получает список ингредиентов.
        /// </summary>
        /// <param name="limit">Ограничение выборки элементов.</param>
        /// <param name="offset">Отступ данных.</param>
        /// <param name="withRelationships">Включать ли связи.</param>
        [HttpGet("")]
        public GetIngredientsResponse GetIngredients(
            [FromQuery] int limit = 25,
            [FromQuery] int offset = 0,
            [FromQuery] bool withRelationships = false)
        {
            var ingredients = _getIngredients.Execute(new GetIngredientsQuery(withRelationships, limit, offset));
            return new GetIngredientsResponse(ingredients);
        }

        /// <summary>
        /// Получает подробную информацию об ингредиенте.
        /// </summary>
        /// <param name="ingredientName">Название ингредиента.</param>
        [HttpGet("{ingredientName}")]
        public IActionResult GetIngredient(
            [FromRoute] string ingredientName)
        {
            var ingredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredientName));
            if (ingredient == null)
                return NotFound();

            return Ok(new GetSingleIngredientResponse(ingredient.Id, ingredient.Name, ingredient.Categories));
        }

        /// <summary>
        /// Удаляет ингредиент по ID.
        /// </summary>
        /// <param name="ingredientId">Идентификатор ингредиента в формате GUID.</param>
        [HttpDelete("{ingredientId}")]
        public IActionResult DeleteIngredient(
            [FromRoute] Guid ingredientId)
        {
            _deleteIngredient.Execute(new DeleteIngredientCommand(ingredientId));
            return Ok();
        }

        /// <summary>
        /// Редактирует название ингредиента по ID.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{ingredientId}")]
        public IActionResult EditIngredient(
            [FromRoute] Guid ingredientId,
            [FromBody] EditIngredientRequest request)
        {
            _editIngredient.Execute(new EditIngredientCommand(ingredientId, request.NewName));
            return Ok();
        }

        /// <summary>
        /// Создает ингредиент.
        /// </summary>
        /// <param name="request">Запрос на создание ингредиента.</param>
        [HttpPost("")]
        public IActionResult CreateIngredient(
            [FromBody] CreateIngredientRequest request)
        {
            _createIngredient.Execute(new CreateIngredientCommand(request.Name, request.Categories));

            var createdIngredient = _searchIngredient.Execute(new SearchIngredientQuery(request.Name));
            if (createdIngredient == null)
                return Conflict("Не удалось создать ингредиент");

            return Created(Url.Action(nameof(GetIngredient), new { ingredientName = createdIngredient.Name }),
                new
                {
                    Url = Url.ActionLink(nameof(GetIngredient), "Ingredient", new { ingredientName = createdIngredient.Name }),
                    createdIngredient.Id
                });

        }

        /// <summary>
        /// Добавляет категорию ингредиенту.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="categoryName">Название категории.</param>
        [HttpPut("{ingredientId}/{categoryName}")]
        public IActionResult AppendCategory([FromRoute] Guid ingredientId, [FromRoute] string categoryName)
        {
            _editor.AppendCategory(categoryName, ingredientId);
            return Ok();
        }

        /// <summary>
        /// Удаляет категорию у ингредиента.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="categoryName">Название категории.</param>
        [HttpDelete("{ingredientId}/{categoryName}")]
        public IActionResult RemoveCategory([FromRoute] Guid ingredientId, [FromRoute] string categoryName)
        {
            _editor.RemoveCategory(categoryName, ingredientId);
            return Ok();
        }
    }
}
