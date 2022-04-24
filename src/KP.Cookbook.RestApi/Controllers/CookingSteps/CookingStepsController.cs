using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Features.RecipeSteps.EditRecipeStep;
using KP.Cookbook.RestApi.Controllers.CookingSteps.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KP.Cookbook.RestApi.Controllers.CookingSteps
{
    [Route("api/steps")]
    public class CookingStepsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<EditRecipeStepCommand> _editRecipeStepCommandHandler;

        public CookingStepsController(
            ICommandHandler<EditRecipeStepCommand> editRecipeStepCommandHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _editRecipeStepCommandHandler = editRecipeStepCommandHandler;
        }

        /// <summary>
        /// Редактирование данных шага по приготовлению рецепта.
        /// </summary>
        [HttpPatch]
        public IActionResult EditRecipeStep([FromBody] EditRecipeStepRequest request) => ExecuteAction(() =>
        {
            _editRecipeStepCommandHandler.Execute(new EditRecipeStepCommand(request.StepId, request.Description, request.ImageBase64));
        });
    }
}
