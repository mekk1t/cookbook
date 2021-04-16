using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class ReplaceIngredientInRecipeDecorator : ICommand<ReplaceRecipeIngredientCommand>
    {
        private readonly ICommand<ReplaceRecipeIngredientCommand> _decoratee;
        private readonly IEntityChecker<Recipe, Guid> _recipeChecker;

        public ReplaceIngredientInRecipeDecorator(
            ICommand<ReplaceRecipeIngredientCommand> decoratee,
            IEntityChecker<Recipe, Guid> recipeChecker)
        {
            _decoratee = decoratee;
            _recipeChecker = recipeChecker;
        }

        public void Execute(ReplaceRecipeIngredientCommand command)
        {
            if (command.OldIngredient == command.NewIngredient)
                return;

            bool recipeExists = _recipeChecker.CheckExistence(command.RecipeId);
            if (!recipeExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
