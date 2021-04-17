using System;

namespace KitProjects.MasterChef.Kernel.Models.EntityChecks
{
    public class StepEntityCheckParameters
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }

        public StepEntityCheckParameters(Guid recipeId, Guid stepId)
        {
            RecipeId = recipeId;
            StepId = stepId;
        }
    }
}
