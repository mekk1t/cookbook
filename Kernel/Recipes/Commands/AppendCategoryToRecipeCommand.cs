using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class AppendCategoryToRecipeCommand
    {
        public Guid CategoryId { get; }
        public Guid RecipeId { get; }

        public AppendCategoryToRecipeCommand(Guid categoryId, Guid recipeId)
        {
            CategoryId = categoryId;
            RecipeId = recipeId;
        }
    }
}
