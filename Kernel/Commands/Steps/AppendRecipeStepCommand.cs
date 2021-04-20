using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class AppendRecipeStepCommand
    {
        public RecipeStep Step { get; }
        public Guid RecipeId { get; }

        public AppendRecipeStepCommand(Guid recipeId, RecipeStep step)
        {
            RecipeId = recipeId;
            Step = step;
        }
    }
}
