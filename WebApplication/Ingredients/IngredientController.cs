using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    [Route("ingredients")]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientService _ingredientService;
        private readonly CategoryService _categoryService;

        public IngredientController(IngredientService ingredientService, CategoryService categoryService)
        {
            _ingredientService = ingredientService;
            _categoryService = categoryService;
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
            var ingredients = _ingredientService.GetIngredients(new GetIngredientsQuery(withRelationships, limit, offset));
            return new GetIngredientsResponse(ingredients);
        }

        /// <summary>
        /// Получает подробную информацию об ингредиенте.
        /// </summary>
        /// <param name="ingredientName">Название ингредиента.</param>
        [HttpGet("{ingredientName}")]
        public IActionResult GetIngredient([FromRoute] string ingredientName)
        {
            var ingredient = _ingredientService.GetIngredients(new GetIngredientsQuery(withRelationships: true)).FirstOrDefault(i => i.Name == ingredientName);
            if (ingredient == null)
                return NotFound();

            return new JsonResult(new GetSingleIngredientResponse(ingredient.Id, ingredient.Name, ingredient.Categories));
        }

        /// <summary>
        /// Удаляет ингредиент по ID.
        /// </summary>
        /// <param name="ingredientId">Идентификатор ингредиента в формате GUID.</param>
        [HttpDelete("{ingredientId}")]
        public IActionResult DeleteIngredient([FromRoute] Guid ingredientId)
        {
            _ingredientService.DeleteIngredient(new DeleteIngredientCommand(ingredientId));
            return Ok();
        }

        /// <summary>
        /// Редактирует ингредиент по ID.
        /// </summary>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPut("{ingredientId}")]
        public IActionResult EditIngredient([FromRoute] Guid ingredientId, [FromBody] EditIngredientRequest request)
        {
            _ingredientService.EditIngredient(new EditIngredientCommand(ingredientId, request.NewName));
            return Ok();
        }

        /// <summary>
        /// Создает ингредиент.
        /// </summary>
        /// <param name="request">Запрос на создание ингредиента.</param>
        [HttpPost("")]
        public IActionResult CreateIngredient([FromBody] CreateIngredientRequest request)
        {
            _ingredientService.CreateIngredient(new CreateIngredientCommand(request.Name, request.Categories));

            var createdIngredient = _ingredientService.GetIngredients(new GetIngredientsQuery()).FirstOrDefault(i => i.Name == request.Name);
            if (createdIngredient == null)
                return Conflict("Не удалось создать ингредиент");

            return Created(Url.Action(nameof(GetIngredient), new { ingredientName = createdIngredient.Name }),
                new
                {
                    Url = Url.ActionLink(nameof(GetIngredient), "Ingredient", new { ingredientName = createdIngredient.Name }),
                    Id = createdIngredient.Id
                });

        }
    }
}
