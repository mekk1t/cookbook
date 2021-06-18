using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class ReplaceIngredientsListInRecipeDecorator : ICommand<ReplaceRecipeIngredientsListCommand>
    {
        private readonly ICommand<ReplaceRecipeIngredientsListCommand> _decoratee;
        private readonly IEntityChecker<Ingredient, string> _ingredientChecker;
        private readonly IEntityChecker<Recipe, Guid> _recipeChecker;
        private readonly ICommand<CreateIngredientCommand> _createIngredient;

        public ReplaceIngredientsListInRecipeDecorator(
            ICommand<ReplaceRecipeIngredientsListCommand> decoratee,
            IEntityChecker<Ingredient, string> ingredientChecker,
            IEntityChecker<Recipe, Guid> recipeChecker,
            ICommand<CreateIngredientCommand> createIngredient)
        {
            _decoratee = decoratee;
            _ingredientChecker = ingredientChecker;
            _recipeChecker = recipeChecker;
            _createIngredient = createIngredient;
        }

        public void Execute(ReplaceRecipeIngredientsListCommand command)
        {
            bool recipeExists = _recipeChecker.CheckExistence(command.RecipeId);
            if (!recipeExists)
                throw new ArgumentException(null, nameof(command));

            foreach (var ingredient in command.NewIngredients)
            {
                bool ingredientExists = _ingredientChecker.CheckExistence(ingredient.Name);
                if (!ingredientExists)
                {
                    _createIngredient.Execute(new CreateIngredientCommand(
                        ingredient.Name,
                        ingredient.Categories.Select(c => c.Name)));
                }
            }

            _decoratee.Execute(command);
        }
    }
}
