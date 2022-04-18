namespace KP.Cookbook.Features.Recipes.DeleteRecipe
{
    public class DeleteRecipeCommand
    {
        public long RecipeId { get; }

        public DeleteRecipeCommand(long recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
