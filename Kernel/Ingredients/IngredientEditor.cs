using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;

namespace KitProjects.MasterChef.Kernel.Ingredients
{
    public class IngredientEditor
    {
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly ICommand<AppendIngredientCategoryCommand> _appendCategory;
        private readonly ICommand<RemoveIngredientCategoryCommand> _removeCategory;

        public IngredientEditor(
            IQuery<Category, SearchCategoryQuery> searchCategory,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            ICommand<AppendIngredientCategoryCommand> appendCategory,
            ICommand<RemoveIngredientCategoryCommand> removeCategory)
        {
            _searchCategory = searchCategory;
            _searchIngredient = searchIngredient;
            _appendCategory = appendCategory;
            _removeCategory = removeCategory;
        }

        public void AppendCategory(string categoryName, Guid ingredientId)
        {
            var appendCategory = _searchCategory.Execute(new SearchCategoryQuery(categoryName));
            if (appendCategory == null)
                throw new InvalidOperationException();
            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredientId));
            if (existingIngredient == null)
                throw new ArgumentException(null, nameof(ingredientId));
            if (existingIngredient.Categories.Contains(appendCategory))
                throw new ArgumentException("Нельзя добавить одну и ту же категорию дважды.");

            _appendCategory.Execute(new AppendIngredientCategoryCommand(categoryName, ingredientId));
        }

        public void RemoveCategory(string categoryName, Guid ingredientId)
        {
            var existingCategory = _searchCategory.Execute(new SearchCategoryQuery(categoryName));
            if (existingCategory == null)
                return;
            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredientId));
            if (existingIngredient == null)
                throw new ArgumentException(null, nameof(ingredientId));

            _removeCategory.Execute(new RemoveIngredientCategoryCommand(categoryName, ingredientId));
        }
    }
}
