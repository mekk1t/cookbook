using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class NormalizeStepsOrderCommand
    {
        public Guid RecipeId { get; }
        public int StartIndex { get; }

        public NormalizeStepsOrderCommand(Guid recipeId, int startIndex)
        {
            RecipeId = recipeId;
            StartIndex = startIndex;
        }
    }
}
