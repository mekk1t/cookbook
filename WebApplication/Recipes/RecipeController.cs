using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    [Produces("application/json")]
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly RecipeEditor _editor;
        private readonly RecipeIngredientEditor _recipeIngredientEditor;

        public RecipeController(RecipeService recipeService, RecipeEditor editor, RecipeIngredientEditor recipeIngredientEditor)
        {
            _recipeService = recipeService;
            _editor = editor;
            _recipeIngredientEditor = recipeIngredientEditor;
        }

        /// <summary>
        /// Создает рецепт.
        /// </summary>
        /// <param name="request">Запрос на создание рецепта.</param>
        [HttpPost("")]
        public IActionResult CreateRecipe([FromBody] CreateRecipeRequest request)
        {
            _recipeService.CreateRecipe(new CreateRecipeCommand(Guid.NewGuid(), request.Title, request.Categories, request.IngredientDetails, request.Steps));
            return Ok();
        }

        /// <summary>
        /// Получает список рецептов. Включает все связи.
        /// </summary>
        [HttpGet("")]
        public IEnumerable<Recipe> GetRecipes() => _recipeService.GetRecipes(new GetRecipesQuery(true));

        [HttpGet("{recipeId}")]
        public RecipeDetails GetRecipe([FromRoute] Guid recipeId) => _recipeService.GetRecipe(recipeId);

        /// <summary>
        /// Редактирует рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID рецепта в формате GUID.</param>
        /// <param name="request">Запрос на редактирование рецепта.</param>
        [HttpPut("{recipeId}")]
        public IActionResult EditRecipe(
            [FromRoute] Guid recipeId,
            [FromBody] EditRecipeRequest request,
            [FromServices] ICommand<EditRecipeCommand> command)
        {
            command.Execute(new EditRecipeCommand(recipeId, request.NewTitle, request.NewDescription));
            return Ok();
        }

        /// <summary>
        /// Удаляет из рецепта категорию по имени.
        /// </summary>
        /// <param name="recipeId">ID рецепта, из которого будет удаляться категория.</param>
        /// <param name="categoryName">Название категории для удаления.</param>
        [HttpDelete("{recipeId}/{categoryName}")]
        public IActionResult RemoveCategory([FromRoute] Guid recipeId, [FromRoute] string categoryName)
        {
            _editor.RemoveCategory(categoryName, recipeId);
            return Ok();
        }

        /// <summary>
        /// Добавляет категорию в рецепт.
        /// </summary>
        /// <param name="recipeId">ID рецепта, куда будет добавлена категория.</param>
        /// <param name="categoryName">Название существующей категории.</param>
        /// <returns></returns>
        [HttpPut("{recipeId}/{categoryName}")]
        public IActionResult AppendCategory([FromRoute] Guid recipeId, [FromRoute] string categoryName)
        {
            _editor.AppendCategory(categoryName, recipeId);
            return Ok();
        }

        /// <summary>
        /// Удаляет рецепт по ID.
        /// </summary>
        /// <param name="recipeId">ID в формате GUID.</param>
        [HttpDelete("{recipeId}")]
        public IActionResult DeleteRecipe([FromRoute] Guid recipeId, [FromServices] ICommand<DeleteRecipeCommand> command)
        {
            command.Execute(new DeleteRecipeCommand(recipeId));
            return Ok();
        }

        /// <summary>
        /// Добавляет ингредиент в рецепт.
        /// </summary>
        /// <returns></returns>
        [HttpPost("{recipeId}/ingredients")]
        public IActionResult AppendIngredient([FromRoute] Guid recipeId, [FromBody] AppendIngredientRequest request)
        {
            _recipeIngredientEditor.AppendIngredient(
                new AppendRecipeIngredientCommand(
                    recipeId,
                    new Ingredient(Guid.NewGuid(), request.IngredientName),
                    new AppendIngredientParameters(request.Amount, request.Measure, request.Notes)));
            return Ok();
        }

        /// <summary>
        /// Редактирует описание ингредиента в рецепте.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="ingredientId">ID ингредиента для редактирования.</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{recipeId}/ingredients/{ingredientId}/description")]
        public IActionResult EditIngredientDescription(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] EditRecipeIngredientDescriptionRequest request)
        {
            _recipeIngredientEditor.EditIngredientsDescription(new EditRecipeIngredientDescriptionCommand(
                recipeId,
                ingredientId,
                request.Amount,
                request.Measure,
                request.Notes));

            return Ok();
        }

        /// <summary>
        /// Удаляет ингредиент из рецепта.
        /// </summary>
        /// <param name="recipeId">ID рецепта, в котором находится искомый ингредиент.</param>
        /// <param name="ingredientId">ID ингредиента на удаление.</param>
        /// <returns></returns>
        [HttpDelete("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult RemoveIngredient([FromRoute] Guid recipeId, [FromRoute] Guid ingredientId)
        {
            _recipeIngredientEditor.RemoveIngredient(recipeId, ingredientId);
            return Ok();
        }

        /// <summary>
        /// Заменяет старый ингредиент на новый. Новый создается, если отсутствует в базе.
        /// </summary>
        /// <param name="recipeId">ID рецепта, в котором находятся ингредиенты.</param>
        /// <param name="ingredientId">ID старого ингредиента, который будет заменен.</param>
        [HttpPut("{recipeId}/ingredients/{ingredientId}")]
        public IActionResult ReplaceIngredient(
            [FromRoute] Guid recipeId,
            [FromRoute] Guid ingredientId,
            [FromBody] ReplaceIngredientRequest request)
        {
            _recipeIngredientEditor.ReplaceIngredient(
                new Ingredient(ingredientId, request.OldIngredientName),
                new Ingredient(Guid.NewGuid(), request.NewIngredientName),
                recipeId);

            return Ok();
        }

        /// <summary>
        /// Заменяет все ингредиенты в рецепте на новые. Создает в процессе несуществующие.
        /// </summary>
        /// <param name="recipeId">ID рецепта.</param>
        /// <param name="request">Запрос на замену ингредиентов.</param>
        [HttpPut("{recipeId}/ingredients")]
        public IActionResult ReplaceIngredients([FromRoute] Guid recipeId, [FromBody] ReplaceIngredientsRequest request)
        {
            _recipeIngredientEditor.ReplaceIngredientsList(
                request.NewIngredients.Select(i => new Ingredient(Guid.NewGuid(), i)).ToArray(),
                recipeId);

            return Ok();
        }
    }
}
