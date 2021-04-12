using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class RemoveRecipeCategoryCommand
    {
        public Guid RecipeId { get; }
        public Guid CategoryId { get; }

        public RemoveRecipeCategoryCommand(Guid recipeId, Guid categoryId)
        {
            RecipeId = recipeId;
            CategoryId = categoryId;
        }
    }
}
