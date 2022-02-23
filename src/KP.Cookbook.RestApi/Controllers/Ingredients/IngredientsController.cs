using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Ingredients.CreateIngredient;
using KP.Cookbook.Features.Ingredients.DeleteIngredient;
using KP.Cookbook.Features.Ingredients.GetIngredients;
using KP.Cookbook.Features.Ingredients.UpdateIngredient;
using KP.Cookbook.RestApi.Controllers.Ingredients.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.Ingredients
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<CreateIngredientCommand, Ingredient> _createIngredient;
        private readonly ICommandHandler<DeleteIngredientCommand> _deleteIngredient;
        private readonly ICommandHandler<UpdateIngredientCommand> _updateIngredient;
        private readonly IQueryHandler<GetIngredientsQuery, List<Ingredient>> _getIngredients;

        public IngredientsController(
            ICommandHandler<CreateIngredientCommand, Ingredient> createIngredient,
            ICommandHandler<DeleteIngredientCommand> deleteIngredient,
            ICommandHandler<UpdateIngredientCommand> updateIngredient,
            IQueryHandler<GetIngredientsQuery, List<Ingredient>> getIngredients,
            ILogger<IngredientsController> logger) : base(logger)
        {
            _createIngredient = createIngredient;
            _deleteIngredient = deleteIngredient;
            _updateIngredient = updateIngredient;
            _getIngredients = getIngredients;
        }

        /// <summary>
        /// Список ингредиентов.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetIngredients() => ExecuteCollectionRequest(() => _getIngredients.Execute(new GetIngredientsQuery()));

        /// <summary>
        /// Создание нового ингредиента.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateIngredient([FromBody] CreateIngredientRequest request) =>
            ExecuteObjectRequest(() => _createIngredient.Execute(new CreateIngredientCommand(new Ingredient(request.Name, request.Type, request.Description))));

        /// <summary>
        /// Редактирование ингредиента.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult UpdateIngredient([FromBody] CreateIngredientRequest request, [FromRoute] long id) =>
            ExecuteAction(() => _updateIngredient.Execute(new UpdateIngredientCommand(new Ingredient(id, request.Name, request.Type, request.Description))));

        /// <summary>
        /// Удаление ингредиента.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteIngredientById([FromRoute] long id) =>
            ExecuteAction(() => _deleteIngredient.Execute(new DeleteIngredientCommand(id)));
    }
}