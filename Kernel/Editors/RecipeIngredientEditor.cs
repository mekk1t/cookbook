using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeIngredientEditor
    {
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;
        private readonly ICommand<ReplaceRecipeIngredientCommand> _replaceIngredient;
        private readonly ICommand<ReplaceIngredientsListCommand> _replaceIngredientsList;
        private readonly ICommand<CreateIngredientCommand> _createIngredient;
        private readonly ICommand<EditRecipeIngredientDescriptionCommand> _editIngredientDescription;

        public RecipeIngredientEditor(
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            IQuery<Recipe, SearchRecipeQuery> searchRecipe,
            ICommand<ReplaceIngredientsListCommand> replaceIngredientsList,
            ICommand<ReplaceRecipeIngredientCommand> replaceIngredient,
            ICommand<CreateIngredientCommand> createIngredient,
            ICommand<EditRecipeIngredientDescriptionCommand> editIngredientDescription)
        {
            _searchIngredient = searchIngredient;
            _searchRecipe = searchRecipe;
            _replaceIngredient = replaceIngredient;
            _replaceIngredientsList = replaceIngredientsList;
            _createIngredient = createIngredient;
            _editIngredientDescription = editIngredientDescription;
        }

        public void ReplaceIngredient(Ingredient oldIngredient, Ingredient newIngredient, Guid recipeId)
        {
            if (oldIngredient == newIngredient)
                return;

            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            _replaceIngredient.Execute(new ReplaceRecipeIngredientCommand(oldIngredient, newIngredient, recipeId));
        }

        public void ReplaceIngredientsList(Ingredient[] newIngredients, Guid recipeId)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            foreach (var ingredient in newIngredients)
            {
                var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredient.Name));
                if (existingIngredient == null)
                {
                    _createIngredient.Execute(new CreateIngredientCommand(
                        ingredient.Name,
                        ingredient.Categories.Select(c => c.Name)));
                }
            }

            _replaceIngredientsList.Execute(new ReplaceIngredientsListCommand(newIngredients, recipeId));
        }

        public void EditIngredientsDescription(EditRecipeIngredientDescriptionCommand command)
        {
            if (!command.HasValues)
                return;

            _editIngredientDescription.Execute(command);
        }
    }
}
