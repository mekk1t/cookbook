using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class AppendCategoryToRecipeCommand
    {
        public string CategoryName { get; }
        public Guid RecipeId { get; }

        public AppendCategoryToRecipeCommand(string categoryName, Guid recipeId)
        {
            CategoryName = categoryName;
            RecipeId = recipeId;
        }
    }
}
