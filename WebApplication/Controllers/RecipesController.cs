using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Requests.Append;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class RecipesController : ApiController
    {
        private readonly RecipeCrud _crud;
        private readonly CategoryManager _categoryManager;
        private readonly RecipeIngredientsManager _ingredientsManager;

        public RecipesController(RecipeCrud crud, CategoryManager categoryManager, RecipeIngredientsManager ingredientsManager)
        {
            _crud = crud;
            _categoryManager = categoryManager;
            _ingredientsManager = ingredientsManager;
        }

        /// <summary>
        /// Получает список рецептов.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiCollectionResponse<Recipe>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult GetRecipes([FromQuery] PaginationFilter filter) =>
            ProcessRequest(() =>
            {
                return new ApiCollectionResponse<Recipe>(_crud.Read(filter));
            });

        /// <summary>
        /// Создает рецепт.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request) =>
            ProcessRequest(() =>
            {
                _crud.Create(
                    new CreateRecipeCommand(
                        Guid.NewGuid(),
                        request.Title,
                        request.Categories,
                        request.Ingredients
                            .Select(details =>
                                new RecipeIngredientDetails(
                                    details.IngredientName,
                                    details.Measure,
                                    details.Amount,
                                    details.Notes)),
                        request.Steps
                            .Select(step =>
                                new RecipeStep
                                {
                                    Description = step.Description,
                                    Image = step.Image
                                })));
                return Ok();
            });

        /// <summary>
        /// Получает подробную информацию о рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet("{recipeId}")]
        [ProducesResponseType(typeof(ApiResponse<RecipeDetails>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult GetRecipe([FromRoute] Guid recipeId) =>
            ProcessRequest(() =>
            {
                return new ApiResponse<RecipeDetails>(_crud.Read(recipeId));
            });

        /// <summary>
        /// Редактирует название и описание рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult EditRecipe([FromRoute] Guid recipeId, [FromBody] EditRecipeRequest request) =>
            ProcessRequest(() => _crud.Update(recipeId, request.NewTitle, request.NewDescription));

        /// <summary>
        /// Удаляет рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId) =>
            ProcessRequest(() => _crud.Delete(recipeId));

        [HttpPost("{recipeId}/categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult AddCategoryToRecipe([FromRoute] Guid recipeId, [FromBody] AppendCategoryToRecipeRequest request) =>
            ProcessRequest(() => _categoryManager.AddToRecipe(recipeId, request.CategoryName));

        [HttpDelete("{recipeId}/categories/{categoryName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult AddCategoryToRecipe([FromRoute] Guid recipeId, [FromRoute] string categoryName) =>
            ProcessRequest(() => _categoryManager.RemoveFromRecipe(recipeId, categoryName));

        [HttpPost("{recipeId}/ingredients")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult AddIngredientToRecipe([FromRoute] Guid recipeId, [FromBody] AppendIngredientRequest request) =>
            ProcessRequest(() => _ingredientsManager.AppendIngredientToRecipe(
                recipeId,
                request.IngredientName,
                new IngredientParameters(
                    request.Amount,
                    request.Measure,
                    request.Notes)));

        [HttpPut("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult ReplaceIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] ReplaceIngredientRequest request) =>
            ProcessRequest(() =>
            {
                _ingredientsManager.ReplaceIngredient(recipeId, ingredientId, request.OldIngredientName, request.NewIngredientName);
            });

        [HttpPatch("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult EditIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] EditIngredientDescriptionRequest request) =>
            ProcessRequest(() =>
            {
                _ingredientsManager.EditIngredientDescription(
                    recipeId,
                    ingredientId,
                    new IngredientParameters(
                        request.Amount,
                        request.Measure,
                        request.Notes));
            });

        [HttpDelete("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] Guid recipeId, [FromRoute] Guid ingredientId) =>
            ProcessRequest(() => _ingredientsManager.RemoveIngredientFromRecipe(recipeId, ingredientId));
    }
}
