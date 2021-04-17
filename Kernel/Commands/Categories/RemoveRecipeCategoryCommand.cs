using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class RemoveRecipeCategoryCommand
    {
        public Guid RecipeId { get; }
        public string CategoryName { get; }

        public RemoveRecipeCategoryCommand(Guid recipeId, string categoryName)
        {
            RecipeId = recipeId;
            CategoryName = categoryName;
        }
    }
}
