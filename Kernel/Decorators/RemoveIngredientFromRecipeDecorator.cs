using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class RemoveIngredientFromRecipeDecorator : ICommand<RemoveIngredientFromRecipeCommand>
    {
        private readonly ICommand<RemoveIngredientFromRecipeCommand> _decoratee;
        private readonly IEntityChecker<Ingredient, Guid> _ingredientChecker;
        private readonly IEntityChecker<Recipe, Guid> _recipeChecker;

        public RemoveIngredientFromRecipeDecorator(
            ICommand<RemoveIngredientFromRecipeCommand> decoratee,
            IEntityChecker<Ingredient, Guid> ingredientChecker,
            IEntityChecker<Recipe, Guid> recipeChecker)
        {
            _decoratee = decoratee;
            _ingredientChecker = ingredientChecker;
            _recipeChecker = recipeChecker;
        }

        public void Execute(RemoveIngredientFromRecipeCommand command)
        {
            bool ingredientExists = _ingredientChecker.CheckExistence(command.IngredientId);
            if (!ingredientExists)
                return;

            bool recipeExists = _recipeChecker.CheckExistence(command.RecipeId);
            if (!recipeExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
