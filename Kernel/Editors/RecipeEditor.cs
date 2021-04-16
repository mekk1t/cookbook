using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeEditor
    {
        private readonly ICommand<RemoveRecipeCategoryCommand> _removeCategory;
        private readonly ICommand<AppendCategoryToRecipeCommand> _appendCategory;
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;

        public RecipeEditor(
            ICommand<AppendCategoryToRecipeCommand> appendCategory,
            ICommand<RemoveRecipeCategoryCommand> removeCategory,
            IQuery<Category, SearchCategoryQuery> searchCategory,
            IQuery<Recipe, SearchRecipeQuery> searchRecipe)
        {
            _appendCategory = appendCategory;
            _removeCategory = removeCategory;
            _searchCategory = searchCategory;
            _searchRecipe = searchRecipe;
        }

        public void AppendCategory(string categoryName, Guid recipeId)
        {
            var appendCategory = _searchCategory.Execute(new SearchCategoryQuery(categoryName));
            if (appendCategory == null)
                throw new InvalidOperationException("Нельзя добавить несуществующую категорию.");
            var existingRecipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (existingRecipe == null)
                throw new ArgumentException($"Рецепта с ID {recipeId} не существует.");
            if (existingRecipe.Categories.Contains(appendCategory))
                throw new ArgumentException("Нельзя добавить одну категорию дважды.");

            _appendCategory.Execute(new AppendCategoryToRecipeCommand(appendCategory.Id, existingRecipe.Id));
        }

        public void RemoveCategory(string categoryName, Guid recipeId)
        {
            var existingCategory = _searchCategory.Execute(new SearchCategoryQuery(categoryName));
            if (existingCategory == null)
                return;

            var existingRecipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (existingRecipe == null)
                throw new ArgumentException($"Рецепта с ID {recipeId} не существует.");

            _removeCategory.Execute(new RemoveRecipeCategoryCommand(recipeId, existingCategory.Id));
        }
    }
}
