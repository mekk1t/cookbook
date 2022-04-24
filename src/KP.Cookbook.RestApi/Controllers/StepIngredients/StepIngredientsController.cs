using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.StepIngredients.GetStepIngredients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.RestApi.Controllers.StepIngredients
{
    [Route("api/steps/{stepId}/ingredients")]
    public class StepIngredientsController : CookbookApiJsonController
    {
        private readonly IQueryHandler<GetStepIngredientsQuery, List<IngredientDetailed>> _getStepIngredientsQueryHandler;

        public StepIngredientsController(
            IQueryHandler<GetStepIngredientsQuery, List<IngredientDetailed>> getStepIngredientsQueryHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _getStepIngredientsQueryHandler = getStepIngredientsQueryHandler;
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
    }
}
