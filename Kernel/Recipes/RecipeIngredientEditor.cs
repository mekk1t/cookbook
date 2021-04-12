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
        private readonly ICommand<AppendRecipeIngredientCommand> _appendIngredient;
        private readonly ICommand<RemoveRecipeIngredientCommand> _removeIngredient;
        private readonly ICommand<ReplaceRecipeIngredientCommand> _replaceIngredient;
        private readonly ICommand<ReplaceIngredientsListCommand> _replaceIngredientsList;
        private readonly IngredientService _ingredientService;

        public RecipeIngredientEditor(
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            IQuery<Recipe, SearchRecipeQuery> searchRecipe,
            ICommand<AppendRecipeIngredientCommand> appendIngredient,
            ICommand<RemoveRecipeIngredientCommand> removeIngredient,
            ICommand<ReplaceIngredientsListCommand> replaceIngredientsList,
            ICommand<ReplaceRecipeIngredientCommand> replaceIngredient,
            IngredientService ingredientService)
        {
            _searchIngredient = searchIngredient;
            _searchRecipe = searchRecipe;
            _appendIngredient = appendIngredient;
            _removeIngredient = removeIngredient;
            _replaceIngredient = replaceIngredient;
            _replaceIngredientsList = replaceIngredientsList;
            _ingredientService = ingredientService;
        }

        public void AppendIngredient(AppendRecipeIngredientCommand command)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(command.RecipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(command.Ingredient.Id));
            if (existingIngredient == null)
                _ingredientService.CreateIngredient(new CreateIngredientCommand(command.Ingredient.Name, Array.Empty<string>()));

            _appendIngredient.Execute(command);
        }

        public void RemoveIngredient(Guid recipeId, Guid ingredientId)
        {
            var ingredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredientId));
            if (ingredient == null)
                return;

            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            _removeIngredient.Execute(new RemoveRecipeIngredientCommand(recipeId, ingredientId));
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
                    _ingredientService.CreateIngredient(new CreateIngredientCommand(
                        ingredient.Name,
                        ingredient.Categories.Select(c => c.Name)));
                }
            }

            _replaceIngredientsList.Execute(new ReplaceIngredientsListCommand(newIngredients, recipeId));
        }
    }
}
