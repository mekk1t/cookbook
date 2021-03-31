using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class DeleteRecipeCommand
    {
        public Guid RecipeId { get; }

        public DeleteRecipeCommand(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
