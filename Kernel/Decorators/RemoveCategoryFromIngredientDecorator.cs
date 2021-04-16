using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;

namespace KitProjects.MasterChef.Kernel.Ingredients
{
    public class RemoveCategoryFromIngredientDecorator : ICommand<RemoveIngredientCategoryCommand>
    {
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly ICommand<RemoveIngredientCategoryCommand> _decoratee;

        public RemoveCategoryFromIngredientDecorator(
            IQuery<Category, SearchCategoryQuery> searchCategory,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            ICommand<RemoveIngredientCategoryCommand> decoratee)
        {
            _searchCategory = searchCategory;
            _searchIngredient = searchIngredient;
            _decoratee = decoratee;
        }

        public void Execute(RemoveIngredientCategoryCommand command)
        {
            var existingCategory = _searchCategory.Execute(new SearchCategoryQuery(command.CategoryName));
            if (existingCategory == null)
                return;
            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(command.IngredientId));
            if (existingIngredient == null)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
