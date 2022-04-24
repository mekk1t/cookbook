using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.RecipeSteps.AddStepsToRecipe;
using KP.Cookbook.Features.RecipeSteps.DeleteRecipeStep;
using KP.Cookbook.Features.RecipeSteps.EditRecipeStep;
using KP.Cookbook.Features.RecipeSteps.GetRecipeSteps;
using KP.Cookbook.RestApi.Controllers.RecipeSteps.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace KP.Cookbook.RestApi.Controllers.RecipeSteps
{
    /// <summary>
    /// Работа с шагами в рецепте.
    /// </summary>
    [Route("api/recipes/{recipeId}/steps")]
    public class RecipeStepsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<AddStepsToRecipeCommand> _addStepsToRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipeStepsQuery, CookingStepsCollection> _getRecipeStepsQueryCommandHandler;
        private readonly ICommandHandler<EditRecipeStepCommand> _editRecipeStepCommandHandler;
        private readonly ICommandHandler<DeleteRecipeStepCommand> _deleteRecipeStepCommandHandler;

        public RecipeStepsController(
            ICommandHandler<AddStepsToRecipeCommand> addStepsToRecipeCommandHandler,
            IQueryHandler<GetRecipeStepsQuery, CookingStepsCollection> getRecipeStepsQueryCommandHandler,
            ICommandHandler<EditRecipeStepCommand> editRecipeStepCommandHandler,
            ICommandHandler<DeleteRecipeStepCommand> deleteRecipeStepCommandHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _addStepsToRecipeCommandHandler = addStepsToRecipeCommandHandler;
            _getRecipeStepsQueryCommandHandler = getRecipeStepsQueryCommandHandler;
            _editRecipeStepCommandHandler = editRecipeStepCommandHandler;
            _deleteRecipeStepCommandHandler = deleteRecipeStepCommandHandler;
        }

        /// <summary>
        /// Список шагов по приготовлению рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet]
        public IActionResult GetRecipeSteps([FromRoute] long recipeId) =>
            ExecuteObjectRequest(() => _getRecipeStepsQueryCommandHandler.Execute(new GetRecipeStepsQuery(recipeId)));

        /// <summary>
        /// Удаление шага из рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="stepId">ID шага.</param>
        [HttpDelete("{stepId}")]
        public IActionResult DeleteRecipeStep([FromRoute] long recipeId, [FromRoute] long stepId) => ExecuteAction(() =>
            _deleteRecipeStepCommandHandler.Execute(new DeleteRecipeStepCommand(recipeId, stepId)));

        /// <summary>
        /// Добавление шагов в рецепт.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на добавление шагов.</param>
        [HttpPost]
        public IActionResult AddStepsToRecipe([FromRoute] long recipeId, [FromBody] AddStepsToRecipeRequest request) =>
            ExecuteAction(() =>
            {
                var stepsCollection = new CookingStepsCollection(request.RecipeSteps.Select(s => new CookingStep(s.Order)
                {
                    Description = s.Description,
                    Image = s.ImageBase64
                }));

                _addStepsToRecipeCommandHandler.Execute(new AddStepsToRecipeCommand(recipeId, stepsCollection));
            });

        /// <summary>
        /// Редактирование данных шага по приготовлению рецепта.
        /// </summary>
        [HttpPatch("api/recipes/steps")]
        public IActionResult EditRecipeStep([FromBody] EditRecipeStepRequest request) => ExecuteAction(() =>
        {
            _editRecipeStepCommandHandler.Execute(new EditRecipeStepCommand(request.StepId, request.Description, request.ImageBase64));
        });
    }
}
