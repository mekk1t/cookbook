using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class RemoveRecipeStepCommand
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }

        public RemoveRecipeStepCommand(Guid recipeId, Guid stepId)
        {
            RecipeId = recipeId;
            StepId = stepId;
        }
    }
}
