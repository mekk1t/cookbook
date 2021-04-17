using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class AppendIngredientToStepCommand
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }
        public Ingredient Ingredient { get; }

        public AppendIngredientToStepCommand(Guid recipeId, Guid stepId, Ingredient ingredient)
        {
            RecipeId = recipeId;
            StepId = stepId;
            Ingredient = ingredient;
        }
    }
}
