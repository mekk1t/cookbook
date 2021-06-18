using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class CategoryManager
    {
        private readonly ICommand<RemoveRecipeCategoryCommand> _removeRecipeCategory;
        private readonly ICommand<AppendCategoryToRecipeCommand> _appendRecipeCategory;

        public CategoryManager(
            ICommand<RemoveRecipeCategoryCommand> removeRecipeCategory,
            ICommand<AppendCategoryToRecipeCommand> appendRecipeCategory)
        {
            _removeRecipeCategory = removeRecipeCategory;
            _appendRecipeCategory = appendRecipeCategory;
        }

        public void AddToRecipe(Guid recipeId, string categoryName) =>
            _appendRecipeCategory.Execute(new AppendCategoryToRecipeCommand(categoryName, recipeId));

        public void RemoveFromRecipe(Guid recipeId, string categoryName) =>
            _removeRecipeCategory.Execute(new RemoveRecipeCategoryCommand(recipeId, categoryName));

        public void AddToIngredient()
        {

        }

        public void RemoveFromIngredient()
        {

        }
    }
}
