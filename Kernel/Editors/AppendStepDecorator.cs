using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class AppendStepDecorator : ICommand<AppendRecipeStepCommand>
    {
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;
        private readonly ICommand<AppendRecipeStepCommand> _decoratee;
        private readonly RecipeIngredientEditor _recipeIngredientEditor;

        public AppendStepDecorator(
            IQuery<Recipe, SearchRecipeQuery> searchRecipe,
            ICommand<AppendRecipeStepCommand> appendStep,
            RecipeIngredientEditor recipeIngredientEditor)
        {
            _searchRecipe = searchRecipe;
            _decoratee = appendStep;
            _recipeIngredientEditor = recipeIngredientEditor;
        }

        public void Execute(AppendRecipeStepCommand command)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(command.RecipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var recipeIngredientNames = recipe.Ingredients.Select(i => i.Name).ToList();
            foreach (var ingredient in command.Step.IngredientsDetails)
            {
                if (!recipeIngredientNames.Contains(ingredient.IngredientName))
                {
                    _recipeIngredientEditor.AppendIngredient(
                        new AppendRecipeIngredientCommand(
                            command.RecipeId,
                            new Ingredient(
                                Guid.NewGuid(),
                                ingredient.IngredientName),
                            new AppendIngredientParameters(ingredient.Amount, ingredient.Measure)));
                }
            }

            _decoratee.Execute(command);
        }
    }
}
