namespace KP.Cookbook.Features.RecipeIngredients.GetRecipeIngredients
{
    public class GetRecipeIngredientsQuery
    {
        public long RecipeId { get; }

        public GetRecipeIngredientsQuery(long recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
