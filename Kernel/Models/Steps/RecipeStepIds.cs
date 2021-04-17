using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class RecipeStepIds
    {
        public Guid Recipe { get; }
        public Guid Step { get; }

        public RecipeStepIds(Guid recipeId, Guid stepId)
        {
            Recipe = recipeId;
            Step = stepId;
        }
    }
}
