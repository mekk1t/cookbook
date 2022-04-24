using KP.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe;
using KP.Cookbook.Features.RecipeIngredients.EditRecipeIngredient;
using KP.Cookbook.Features.RecipeIngredients.GetRecipeIngredients;
using KP.Cookbook.Features.RecipeIngredients.RemoveIngredientFromRecipe;
using KP.Cookbook.RestApi.Controllers.RecipeIngredients.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.RecipeIngredients
{
    /// <summary>
    /// Работа с ингредиентами в рецепте.
    /// </summary>
    [Route("api/recipes/{recipeId}")]
    public class RecipeIngredientsController : CookbookApiJsonController
    {
        private readonly ICommandHandler<AddIngredientsToRecipeCommand> _addIngredientsToRecipeCommandHandler;
        private readonly IQueryHandler<GetRecipeIngredientsQuery, List<IngredientDetailed>> _getRecipeIngredientsQueryHandler;
        private readonly ICommandHandler<RemoveIngredientFromRecipeCommand> _removeIngredientFromRecipeCommandHandler;
        private readonly ICommandHandler<EditRecipeIngredientCommand> _editRecipeIngredientCommandCommandHandler;

        public RecipeIngredientsController(
            ICommandHandler<AddIngredientsToRecipeCommand> addIngredientsToRecipeCommandHandler,
            IQueryHandler<GetRecipeIngredientsQuery, List<IngredientDetailed>> getRecipeIngredientsQueryHandler,
            ICommandHandler<RemoveIngredientFromRecipeCommand> removeIngredientFromRecipeCommandHandler,
            ICommandHandler<EditRecipeIngredientCommand> editRecipeIngredientCommandCommandHandler,
            ILogger<ApiJsonController> logger) : base(logger)
        {
            _addIngredientsToRecipeCommandHandler = addIngredientsToRecipeCommandHandler;
            _getRecipeIngredientsQueryHandler = getRecipeIngredientsQueryHandler;
            _removeIngredientFromRecipeCommandHandler = removeIngredientFromRecipeCommandHandler;
            _editRecipeIngredientCommandCommandHandler = editRecipeIngredientCommandCommandHandler;
        }

        /// <summary>
        /// Ингредиенты рецепта.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpGet("ingredients")]
        public IActionResult GetRecipeIngredients([FromRoute] long recipeId) =>
            ExecuteCollectionRequest(() => _getRecipeIngredientsQueryHandler.Execute(new GetRecipeIngredientsQuery(recipeId)));

        /// <summary>
        /// Добавление ингредиентов в рецепт.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ingredients")]
        public IActionResult AddIngredientsToRecipe([FromRoute] long recipeId, [FromBody] AddIngredientsToRecipeRequest request) =>
            ExecuteAction(() => _addIngredientsToRecipeCommandHandler.Execute(new AddIngredientsToRecipeCommand(recipeId, request.Ingredients)));

        /// <summary>
        /// Редактирование ингредиента в рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на редактирование.</param>
        [HttpPatch("ingredients")]
        public IActionResult EditRecipeIngredient([FromRoute] long recipeId, [FromBody] EditRecipeIngredientRequest request) =>
            ExecuteAction(() => _editRecipeIngredientCommandCommandHandler.Execute(new EditRecipeIngredientCommand(recipeId, request.Ingredient)));

        /// <summary>
        /// Удаление ингредиента из рецепта.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="ingredientId"></param>
        /// <returns></returns>
        [HttpDelete("ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] long recipeId, [FromRoute] long ingredientId) =>
            ExecuteAction(() => _removeIngredientFromRecipeCommandHandler.Execute(new RemoveIngredientFromRecipeCommand(recipeId, ingredientId)));
    }
}
