using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class AppendCategoryToIngredientDecorator : ICommand<AppendIngredientCategoryCommand>
    {
        private readonly IEntityChecker<Category, string> _categoryChecker;
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;
        private readonly ICommand<AppendIngredientCategoryCommand> _decoratee;

        public AppendCategoryToIngredientDecorator(
            IEntityChecker<Category, string> categoryChecker,
            IQuery<Ingredient, GetIngredientQuery> getIngredient,
            ICommand<AppendIngredientCategoryCommand> decoratee)
        {
            _categoryChecker = categoryChecker;
            _getIngredient = getIngredient;
            _decoratee = decoratee;
        }

        public void Execute(AppendIngredientCategoryCommand command)
        {
            bool categoryExists = _categoryChecker.CheckExistence(command.CategoryName);
            if (!categoryExists)
                throw new InvalidOperationException();

            var existingIngredient = _getIngredient.Execute(new GetIngredientQuery(command.IngredientId));
            if (existingIngredient == null)
                throw new ArgumentException(null, nameof(command));

            if (existingIngredient.Categories.Select(c => c.Name).Contains(command.CategoryName))
                throw new ArgumentException("Нельзя добавить одну и ту же категорию дважды.");

            _decoratee.Execute(command);
        }
    }
}
