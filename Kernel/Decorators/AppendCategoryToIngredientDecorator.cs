using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class AppendCategoryToIngredientDecorator : ICommand<AppendIngredientCategoryCommand>
    {
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly ICommand<AppendIngredientCategoryCommand> _decoratee;

        public AppendCategoryToIngredientDecorator(
            IQuery<Category, SearchCategoryQuery> searchCategory,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            ICommand<AppendIngredientCategoryCommand> decoratee)
        {
            _searchCategory = searchCategory;
            _searchIngredient = searchIngredient;
            _decoratee = decoratee;
        }

        public void Execute(AppendIngredientCategoryCommand command)
        {
            var appendCategory = _searchCategory.Execute(new SearchCategoryQuery(command.CategoryName));
            if (appendCategory == null)
                throw new InvalidOperationException();
            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(command.IngredientId));
            if (existingIngredient == null)
                throw new ArgumentException(null, nameof(command));
            if (existingIngredient.Categories.Contains(appendCategory))
                throw new ArgumentException("Нельзя добавить одну и ту же категорию дважды.");

            _decoratee.Execute(command);
        }
    }
}
