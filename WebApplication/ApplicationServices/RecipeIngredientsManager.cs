using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class RecipeIngredientsManager
    {
        private readonly ICommand<AppendIngredientToRecipeCommand> _appendIngredient;
        private readonly ICommand<EditRecipeIngredientDescriptionCommand> _editIngredientDescription;
        private readonly ICommand<RemoveIngredientFromRecipeCommand> _removeIngredient;
        private readonly ICommand<ReplaceRecipeIngredientsListCommand> _replaceIngredientsList;
        private readonly ICommand<ReplaceRecipeIngredientCommand> _replaceIngredient;

        public RecipeIngredientsManager(
            ICommand<AppendIngredientToRecipeCommand> appendIngredient,
            ICommand<EditRecipeIngredientDescriptionCommand> editIngredientDescription,
            ICommand<RemoveIngredientFromRecipeCommand> removeIngredient,
            ICommand<ReplaceRecipeIngredientsListCommand> replaceIngredientsList,
            ICommand<ReplaceRecipeIngredientCommand> replaceIngredient)
        {
            _appendIngredient = appendIngredient;
            _editIngredientDescription = editIngredientDescription;
            _removeIngredient = removeIngredient;
            _replaceIngredientsList = replaceIngredientsList;
            _replaceIngredient = replaceIngredient;
        }

        public void AppendIngredientToRecipe(Guid recipeId, string ingredientName, IngredientParameters parameters) =>
            _appendIngredient.Execute(new AppendIngredientToRecipeCommand(recipeId, new Ingredient(ingredientName), parameters));

        public void EditIngredientDescription(Guid recipeId, Guid ingredientId, IngredientParameters parameters) =>
            _editIngredientDescription.Execute(new EditRecipeIngredientDescriptionCommand(
                recipeId,
                ingredientId,
                parameters.Amount,
                parameters.Measure,
                parameters.Notes));

        public void RemoveIngredientFromRecipe(Guid recipeId, Guid ingredientId) =>
            _removeIngredient.Execute(new RemoveIngredientFromRecipeCommand(recipeId, ingredientId));
    }
}
