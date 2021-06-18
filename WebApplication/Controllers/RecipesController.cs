using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.WebApplication.ApplicationServices;
using KitProjects.MasterChef.WebApplication.Controllers;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        /// Создает рецепт.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            _crud.Create(
                new CreateRecipeCommand(
                    Guid.NewGuid(),
                    request.Title,
                    request.Categories,
                    request.Ingredients.Select(details => new RecipeIngredientDetails(details.IngredientName, details.Measure, details.Amount, details.Notes)),
                    request.Steps.Select(step => new RecipeStep
                    {
                        Description = step.Description,
                        Image = step.Image
                    })));
            return Ok();
        }

        /// <summary>
        /// Получает список рецептов. Включает все связи.
        /// </summary>
        [HttpGet]
        public IEnumerable<Recipe> GetRecipes(PaginationFilter filter) => _crud.Read(filter);

        /// <summary>
        /// Получает подробную информацию о рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        [HttpGet("{recipeId}")]
        public RecipeDetails GetRecipe([FromRoute] Guid recipeId) => _crud.Read(recipeId);

        /// <summary>
        /// Редактирует название и описание рецепта по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        public IActionResult EditRecipe(
            [FromRoute] Guid recipeId,
            [FromBody] EditRecipeRequest request)
        {
            _crud.Update(recipeId, request.NewTitle, request.NewDescription);
            return Ok();
        }

        /// <summary>
        /// Удаляет рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId)
        {
            _crud.Delete(recipeId);
            return Ok();
        }
    }
}
