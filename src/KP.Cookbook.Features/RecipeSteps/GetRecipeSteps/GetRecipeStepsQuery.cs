namespace KP.Cookbook.Features.RecipeSteps.GetRecipeSteps
{
    public class GetRecipeStepsQuery
    {
        public long RecipeId { get; }

        public GetRecipeStepsQuery(long recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
