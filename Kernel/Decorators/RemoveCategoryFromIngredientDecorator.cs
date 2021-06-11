using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Ingredients
{
    public class RemoveCategoryFromIngredientDecorator : ICommand<RemoveIngredientCategoryCommand>
    {
        private readonly IEntityChecker<Category, Guid> _categoryChecker;
        private readonly IEntityChecker<Ingredient, Guid> _ingredientChecker;
        private readonly ICommand<RemoveIngredientCategoryCommand> _decoratee;

        public RemoveCategoryFromIngredientDecorator(
            IEntityChecker<Category, Guid> categoryChecker,
            IEntityChecker<Ingredient, Guid> ingredientChecker,
            ICommand<RemoveIngredientCategoryCommand> decoratee)
        {
            _categoryChecker = categoryChecker;
            _ingredientChecker = ingredientChecker;
            _decoratee = decoratee;
        }

        public void Execute(RemoveIngredientCategoryCommand command)
        {
            bool categoryExists = _categoryChecker.CheckExistence(command.CategoryId);
            if (!categoryExists)
                return;

            bool ingredientExists = _ingredientChecker.CheckExistence(command.IngredientId);
            if (!ingredientExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
