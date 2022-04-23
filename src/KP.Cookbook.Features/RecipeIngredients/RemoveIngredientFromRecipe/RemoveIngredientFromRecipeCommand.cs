namespace KP.Cookbook.Features.RecipeIngredients.RemoveIngredientFromRecipe
{
    public class RemoveIngredientFromRecipeCommand
    {
        public long RecipeId { get; }
        public long IngredientId { get; }

        public RemoveIngredientFromRecipeCommand(long recipeId, long ingredientId)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }
    }
}
