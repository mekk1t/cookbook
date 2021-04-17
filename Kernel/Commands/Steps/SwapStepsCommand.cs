using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class SwapStepsCommand
    {
        public Guid RecipeId { get; }
        public Guid FirstStepId { get; }
        public Guid SecondStepId { get; }

        public SwapStepsCommand(Guid firstStepId, Guid secondStepId, Guid recipeId)
        {
            FirstStepId = firstStepId;
            SecondStepId = secondStepId;
            RecipeId = recipeId;
        }
    }
}
