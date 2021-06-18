using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class RecipesController : ApiController
    {
        private readonly RecipeCrud _crud;

        public RecipesController(RecipeCrud crud)
        {
            _crud = crud;
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
    }
}
