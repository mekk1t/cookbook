namespace KP.Cookbook.Features.Recipes.GetRecipeDetails
{
    public class GetRecipeDetailsQuery
    {
        public long RecipeId { get; }

        public GetRecipeDetailsQuery(long recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
