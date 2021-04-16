using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class RemoveCategoryFromRecipeDecorator : ICommand<RemoveRecipeCategoryCommand>
    {
        private readonly IEntityChecker<Category, string> _categoryChecker;
        private readonly IEntityChecker<Recipe, Guid> _recipeChecker;
        private readonly ICommand<RemoveRecipeCategoryCommand> _decoratee;

        public RemoveCategoryFromRecipeDecorator(
            IEntityChecker<Category, string> categoryChecker,
            IEntityChecker<Recipe, Guid> recipeChecker,
            ICommand<RemoveRecipeCategoryCommand> decoratee)
        {
            _categoryChecker = categoryChecker;
            _recipeChecker = recipeChecker;
            _decoratee = decoratee;
        }

        public void Execute(RemoveRecipeCategoryCommand command)
        {
            bool categoryExists = _categoryChecker.CheckExistence(command.CategoryName);
            if (!categoryExists)
                return;

            bool recipeExists = _recipeChecker.CheckExistence(command.RecipeId);
            if (!recipeExists)
                throw new ArgumentException($"Рецепта с ID {command.RecipeId} не существует.");

            _decoratee.Execute(command);
        }
    }
}
