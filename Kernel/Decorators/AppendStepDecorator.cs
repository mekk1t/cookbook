using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class AppendStepDecorator : ICommand<AppendRecipeStepCommand>
    {
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;
        private readonly ICommand<AppendRecipeStepCommand> _decoratee;
        private readonly ICommand<AppendIngredientToRecipeCommand> _appendIngredientToRecipe;

        public AppendStepDecorator(
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe,
            ICommand<AppendRecipeStepCommand> appendStep,
            ICommand<AppendIngredientToRecipeCommand> appendIngredientToRecipe)
        {
            _getRecipe = getRecipe;
            _decoratee = appendStep;
            _appendIngredientToRecipe = appendIngredientToRecipe;
        }

        public void Execute(AppendRecipeStepCommand command)
        {
            var recipe = _getRecipe.Execute(new GetRecipeQuery(command.RecipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var recipeIngredientNames = recipe.Ingredients.Select(i => i.IngredientName).ToList();
            foreach (var ingredient in command.Step.IngredientsDetails)
            {
                if (!recipeIngredientNames.Contains(ingredient.IngredientName))
                {
                    _appendIngredientToRecipe.Execute(
                        new AppendIngredientToRecipeCommand(
                            command.RecipeId,
                            new Ingredient(
                                Guid.NewGuid(),
                                ingredient.IngredientName),
                            new IngredientParameters(ingredient.Amount, ingredient.Measure)));
                }
            }

            _decoratee.Execute(command);
        }
    }
}
