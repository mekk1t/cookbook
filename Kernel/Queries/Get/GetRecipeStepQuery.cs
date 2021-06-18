using System;

namespace KitProjects.MasterChef.Kernel.Queries.Get
{
    public class GetRecipeStepQuery
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }

        public GetRecipeStepQuery(Guid recipeId, Guid stepId)
        {
            RecipeId = recipeId;
            StepId = stepId;
        }
    }
}
