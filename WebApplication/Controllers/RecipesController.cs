using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using KitProjects.MasterChef.WebApplication.Models.Requests.Append;
using KitProjects.MasterChef.WebApplication.Models.Requests.Edit;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using KitProjects.MasterChef.WebApplication.RecipeSteps;
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
        private readonly RecipeStepsManager _stepsManager;
        private readonly RecipeStepIngredientsManager _stepIngredientsManager;

        public RecipesController(
            RecipeCrud crud,
            CategoryManager categoryManager,
            RecipeIngredientsManager ingredientsManager,
            RecipeStepsManager stepsManager,
            RecipeStepIngredientsManager stepIngredientsManager)
        {
            _crud = crud;
            _categoryManager = categoryManager;
            _ingredientsManager = ingredientsManager;
            _stepsManager = stepsManager;
            _stepIngredientsManager = stepIngredientsManager;
        }

        /// <summary>
        /// Список рецептов.
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
        /// Создание рецепта.
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
        /// Подробная информация о рецепте.
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
        /// Редактирование названия и описания рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult EditRecipe([FromRoute] Guid recipeId, [FromBody] EditRecipeRequest request) =>
            ProcessRequest(() => _crud.Update(recipeId, request.NewTitle, request.NewDescription));

        /// <summary>
        /// Удаление рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId) =>
            ProcessRequest(() => _crud.Delete(recipeId));

        /// <summary>
        /// Добавление новой категории рецепту.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на добавление категории.</param>
        /// <returns></returns>
        [HttpPost("{recipeId}/categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult AddCategoryToRecipe([FromRoute] Guid recipeId, [FromBody] AppendCategoryToRecipeRequest request) =>
            ProcessRequest(() => _categoryManager.AddToRecipe(recipeId, request.CategoryName));

        /// <summary>
        /// Удаление категории у рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="categoryName">Название категории, которая будет удалена из рецепта.</param>
        /// <returns></returns>
        [HttpDelete("{recipeId}/categories/{categoryName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public IActionResult AddCategoryToRecipe([FromRoute] Guid recipeId, [FromRoute] string categoryName) =>
            ProcessRequest(() => _categoryManager.RemoveFromRecipe(recipeId, categoryName));

        /// <summary>
        /// Добавление ингредиента в рецепт.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на добавление ингредиента.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Замена текущего списка ингредиентов в рецепте на новый.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на замену ингредиентов.</param>
        /// <returns></returns>
        [HttpPut("{recipeId}/ingredients")]
        public IActionResult ReplaceIngredientsList([FromRoute] Guid recipeId, [FromBody] ReplaceIngredientsRequest request) =>
            ProcessRequest(() =>
            {
                _ingredientsManager.ReplaceIngredientsList(recipeId, request.NewIngredients);
            });

        /// <summary>
        /// Замена одного ингредиента в рецепте на другой.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="ingredientId">ID старого ингредиента, который будет заменен.</param>
        /// <param name="request">Запрос на замену ингредиента.</param>
        /// <returns></returns>
        [HttpPut("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult ReplaceIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] ReplaceIngredientRequest request) =>
            ProcessRequest(() =>
            {
                _ingredientsManager.ReplaceIngredient(recipeId, ingredientId, request.OldIngredientName, request.NewIngredientName);
            });

        /// <summary>
        /// Редактирование описания ингредиента в рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <param name="request">Запрос на редактирование описания.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление ингредиента из рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="ingredientId">ID ингредиента в формате GUID.</param>
        /// <returns></returns>
        [HttpDelete("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult RemoveIngredientFromRecipe([FromRoute] Guid recipeId, [FromRoute] Guid ingredientId) =>
            ProcessRequest(() => _ingredientsManager.RemoveIngredientFromRecipe(recipeId, ingredientId));

        /// <summary>
        /// Добавление шага приготовления в рецепт.
        /// </summary>
        [HttpPost("{recipeId}/steps")]
        public IActionResult AddStepToRecipe([FromRoute] Guid recipeId, [FromBody] AppendStepRequest request) =>
            ProcessRequest(() =>
            {
                _stepsManager.AddStepToRecipe(recipeId, request);
            });

        /// <summary>
        /// Перестановка двух шагов местами.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{recipeId}/steps/swap")]
        public IActionResult SwapStepsPlaces([FromRoute] Guid recipeId, [FromBody] SwapStepsRequest request) =>
            ProcessRequest(() =>
            {
                _stepsManager.SwapSteps(recipeId, request.FirstStepId, request.SecondStepId);
            });

        /// <summary>
        /// Замена старого шага на новый.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="stepId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{recipeId}/steps/{stepId}")]
        public IActionResult ReplaceStep(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromBody] ReplaceStepRequest request) =>
            ProcessRequest(() =>
            {
                _stepsManager.ReplaceStep(recipeId, stepId, request);
            });

        /// <summary>
        /// Редактирование информации о шаге рецепта.
        /// </summary>
        /// <remarks>
        /// Редактирование осуществляется для заданных полей в запросе. Их значение должно быть не null.
        /// </remarks>
        /// <param name="recipeId"></param>
        /// <param name="stepId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{recipeId}/steps/{stepId}")]
        public IActionResult EditStep(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromBody] EditStepRequest request) =>
            ProcessRequest(() =>
            {
                if (request.Description != null)
                    _stepsManager.EditStepDescription(recipeId, stepId, request.Description);

                if (request.PictureBase64 != null)
                    _stepsManager.EditStepPicture(recipeId, stepId, request.PictureBase64);
            });

        /// <summary>
        /// Удаление шага из рецепта.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        [HttpDelete("{recipeId}/steps/{stepId}")]
        public IActionResult DeleteRecipeStep([FromRoute] Guid recipeId, [FromRoute] Guid stepId) =>
            ProcessRequest(() =>
            {
                _stepsManager.DeleteStep(recipeId, stepId);
            });

        /// <summary>
        /// Добавление ингредиента в шаг приготовления.
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="stepId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{recipeId}/steps/{stepId}/ingredients")]
        public IActionResult AddIngredientToStep(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId,
            [FromRoute] AppendIngredientRequest request) =>
            ProcessRequest(() =>
            {
                _stepIngredientsManager.AddIngredientToStep(recipeId, stepId, request);
            });
    }
}
