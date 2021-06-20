﻿using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Steps;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.RecipeSteps
{
    [Route("recipes/{recipeId}/steps")]
    public class RecipeStepController : ControllerBase
    {
        /// <summary>
        /// Заменяет старый шаг на новый.
        /// </summary>
        /// <param name="stepId">ID шага.</param>
        /// <returns></returns>
        [HttpPut("{stepId}")]
        public IActionResult ReplaceStep(
            [FromBody] ReplaceStepRequest request,
            [FromRoute] Guid recipeId,
            [FromRoute] Guid stepId)
        {
            _replaceStep.Execute(new ReplaceStepCommand(
                recipeId,
                stepId,
                request.Description,
                request.Image,
                request.Ingredients.Select(i => new StepIngredientDetails
                {
                    IngredientName = i.IngredientName,
                    Measure = i.Measure,
                    Amount = i.Amount
                }).ToList()));
            return Ok();
        }

        /// <summary>
        /// Меняет содержимое картинки в шаге рецепта.
        /// </summary>
        /// <param name="pictureBase64">Изображение в кодировке base64.</param>
        [HttpPut("{stepId}/picture")]
        public IActionResult ChangeStepPicture(
            [FromBody] string pictureBase64,
            [FromRoute] Guid stepId,
            [FromRoute] Guid recipeId)
        {
            _editPicture.Execute(new EditStepPictureCommand(pictureBase64, stepId, recipeId));
            return Ok();
        }

        /// <summary>
        /// Редактирует описание шага в рецепте.
        /// </summary>
        /// <param name="description">Новое описание шага по приготовлению.</param>
        [HttpPut("{stepId}/description")]
        public IActionResult ChangeStepDescription(
            [FromBody] string description,
            [FromRoute] Guid stepId,
            [FromRoute] Guid recipeId)
        {
            _editDescription.Execute(new EditStepDescriptionCommand(description, stepId, recipeId));
            return Ok();
        }
    }
}
