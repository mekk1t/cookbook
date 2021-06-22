﻿using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using KitProjects.MasterChef.WebApplication.Recipes;
using KitProjects.MasterChef.WebApplication.Recipes.Requests;
using System;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class RecipeStepIngredientsManager
    {
        private readonly ICommand<ReplaceStepIngredientCommand> _replaceStepIngredient;
        private readonly ICommand<EditStepIngredientDescriptionCommand> _editStepIngredientDescription;
        private readonly ICommand<DeleteIngredientFromStepCommand> _deleteIngredient;
        private readonly ICommand<AppendIngredientToStepCommand> _appendIngredient;

        public RecipeStepIngredientsManager(
            ICommand<ReplaceStepIngredientCommand> replaceStepIngredient,
            ICommand<EditStepIngredientDescriptionCommand> editStepIngredientDescription,
            ICommand<DeleteIngredientFromStepCommand> deleteIngredient,
            ICommand<AppendIngredientToStepCommand> appendIngredient)
        {
            _replaceStepIngredient = replaceStepIngredient;
            _editStepIngredientDescription = editStepIngredientDescription;
            _deleteIngredient = deleteIngredient;
            _appendIngredient = appendIngredient;
        }

        public void AddIngredientToStep(Guid recipeId, Guid stepId, AppendIngredientRequest request) =>
            _appendIngredient.Execute(new AppendIngredientToStepCommand(
                new RecipeStepIds(recipeId, stepId),
                new Ingredient(request.IngredientName),
                new IngredientParameters(
                    request.Amount,
                    request.Measure,
                    request.Notes)));

        public void ReplaceIngredient(Guid recipeId, Guid stepId, Guid ingredientId, ReplaceIngredientRequest request) =>
            _replaceStepIngredient.Execute(new ReplaceStepIngredientCommand(
                new RecipeStepIds(recipeId, stepId),
                new Ingredient(ingredientId, request.OldIngredientName),
                new Ingredient(request.NewIngredientName)));

        public void EditIngredient(Guid recipeId, Guid stepId, Guid ingredientId, EditIngredientDescriptionRequest request) =>
            _editStepIngredientDescription.Execute(new EditStepIngredientDescriptionCommand(
                new RecipeStepIds(recipeId, stepId),
                ingredientId,
                request.Amount,
                request.Measure));

        public void RemoveIngredientFromStep(Guid recipeId, Guid stepId, Guid ingredientId) =>
            _deleteIngredient.Execute(new DeleteIngredientFromStepCommand(new RecipeStepIds(recipeId, stepId), ingredientId));
    }
}