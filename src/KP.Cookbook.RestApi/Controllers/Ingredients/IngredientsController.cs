using KitProjects.Api.AspNetCore;
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
    public class IngredientsController : ApiJsonController
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

        [HttpGet]
        public List<Ingredient> GetIngredients() => _getIngredients.Execute(new GetIngredientsQuery());

        [HttpPost]
        public Ingredient CreateIngredient([FromBody] CreateIngredientRequest request) =>
            _createIngredient.Execute(new CreateIngredientCommand(new Ingredient(request.Name, request.Type, request.Description)));

        [HttpPatch("{id}")]
        public void UpdateIngredient([FromBody] CreateIngredientRequest request, [FromRoute] long id) =>
            _updateIngredient.Execute(new UpdateIngredientCommand(new Ingredient(id, request.Name, request.Type, request.Description)));

        [HttpDelete("{id}")]
        public void DeleteIngredientById([FromRoute] long id) => _deleteIngredient.Execute(new DeleteIngredientCommand(id));
    }
}