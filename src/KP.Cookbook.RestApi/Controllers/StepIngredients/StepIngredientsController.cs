using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.RecipeIngredients.Dtos;
using KP.Cookbook.Features.StepIngredients.AddIngredientsToStep;
using KP.Cookbook.Features.StepIngredients.EditStepIngredient;
using KP.Cookbook.Features.StepIngredients.GetStepIngredients;
using KP.Cookbook.Features.StepIngredients.RemoveIngredientFromStep;
using KP.Cookbook.RestApi.Controllers.StepIngredients.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.StepIngredients
{
    [Route("api/steps/{stepId}/ingredients")]
    public class StepIngredientsController : CookbookApiJsonController
    {
        private readonly IQueryHandler<GetStepIngredientsQuery, List<IngredientDetailed>> _getStepIngredientsQueryHandler;
        private readonly ICommandHandler<RemoveIngredientFromStepCommand> _removeIngredientFromStepCommandHandler;
        private readonly ICommandHandler<AddIngredientsToStepCommand> _addIngredientsToStepCommandHandler;
        private readonly ICommandHandler<EditStepIngredientCommand> _editStepIngredientCommandCommandHandler;

        public StepIngredientsController(
            ICommandHandler<EditStepIngredientCommand> editStepIngredientCommandCommandHandler,
            ICommandHandler<AddIngredientsToStepCommand> addIngredientsToStepCommandHandler,
            ICommandHandler<RemoveIngredientFromStepCommand> removeIngredientFromStepCommandHandler,
            IQueryHandler<GetStepIngredientsQuery, List<IngredientDetailed>> getStepIngredientsQueryHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _getStepIngredientsQueryHandler = getStepIngredientsQueryHandler;
            _removeIngredientFromStepCommandHandler = removeIngredientFromStepCommandHandler;
            _addIngredientsToStepCommandHandler = addIngredientsToStepCommandHandler;
            _editStepIngredientCommandCommandHandler = editStepIngredientCommandCommandHandler;
        }

        /// <summary>
        /// Список ингредиентов шага приготовления.
        /// </summary>
        /// <param name="stepId">ID шага.</param>
        [HttpGet]
        public IActionResult GetStepIngredients([FromRoute] long stepId) => ExecuteCollectionRequest(() =>
        {
            return _getStepIngredientsQueryHandler.Execute(new GetStepIngredientsQuery(stepId));
        });

        /// <summary>
        /// Удаление ингредиента из шага.
        /// </summary>
        /// <param name="stepId">ID шага.</param>
        /// <param name="ingredientId">ID ингредиента.</param>
        /// <returns></returns>
        [HttpDelete("{ingredientId}")]
        public IActionResult RemoveIngredientFromStep([FromRoute] long stepId, [FromRoute] long ingredientId) => ExecuteAction(() =>
        {
            _removeIngredientFromStepCommandHandler.Execute(new RemoveIngredientFromStepCommand(stepId, ingredientId));
        });

        /// <summary>
        /// Добавление ингредиентов в шаг приготовления.
        /// </summary>
        /// <param name="stepId">ID шага.</param>
        /// <param name="request">Запрос на добавление ингредиентов.</param>
        [HttpPost]
        public IActionResult AddIngredientsToStep([FromRoute] long stepId, [FromBody] AddIngredientsToStepRequest request) =>
            ExecuteAction(() => _addIngredientsToStepCommandHandler.Execute(new AddIngredientsToStepCommand(request.RecipeId, stepId, request.Ingredients)));

        /// <summary>
        /// Редактирование данных ингредиента в шаге.
        /// </summary>
        /// <param name="stepId">ID шага.</param>
        /// <param name="request">Запрос на редактирование.</param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult EditStepIngredient([FromRoute] long stepId, [FromBody] EditStepIngredientRequest request) =>
            ExecuteAction(() => _editStepIngredientCommandCommandHandler.Execute(new EditStepIngredientCommand(stepId, new RecipeIngredientDto
            {
                Id = request.IngredientId,
                Amount = request.Amount,
                AmountType = request.AmountType,
                IsOptional = request.IsOptional
            })));
    }
}
