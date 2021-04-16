using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class AppendCategoryToRecipeDecorator : ICommand<AppendCategoryToRecipeCommand>
    {
        private readonly IEntityChecker<Category, string> _categoryChecker;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;
        private readonly ICommand<AppendCategoryToRecipeCommand> _decoratee;

        public AppendCategoryToRecipeDecorator(
            IEntityChecker<Category, string> categoryChecker,
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe,
            ICommand<AppendCategoryToRecipeCommand> decoratee)
        {
            _categoryChecker = categoryChecker;
            _getRecipe = getRecipe;
            _decoratee = decoratee;
        }

        public void Execute(AppendCategoryToRecipeCommand command)
        {
            bool categoryExists = _categoryChecker.CheckExistence(command.CategoryName);
            if (!categoryExists)
                throw new InvalidOperationException("Нельзя добавить несуществующую категорию.");

            var existingRecipe = _getRecipe.Execute(new GetRecipeQuery(command.RecipeId));
            if (existingRecipe == null)
                throw new ArgumentException($"Рецепта с ID {command.RecipeId} не существует.");

            if (existingRecipe.Categories.Select(c => c.Name).Contains(command.CategoryName))
                throw new ArgumentException("Нельзя добавить одну категорию дважды.");

            _decoratee.Execute(command);
        }
    }
}
