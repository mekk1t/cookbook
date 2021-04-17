using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class RemoveIngredientFromStepCommand
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }
        public Guid IngredientId { get; }

        public RemoveIngredientFromStepCommand(Guid recipeId, Guid stepId, Guid ingredientId)
        {
            RecipeId = recipeId;
            StepId = stepId;
            IngredientId = ingredientId;
        }
    }
}
