namespace KP.Cookbook.Features.RecipeSteps.DeleteRecipeStep
{
    public class DeleteRecipeStepCommand
    {
        public long RecipeId { get; }
        public long StepId { get; }

        public DeleteRecipeStepCommand(long recipeId, long stepId)
        {
            RecipeId = recipeId;
            StepId = stepId;
        }
    }
}
