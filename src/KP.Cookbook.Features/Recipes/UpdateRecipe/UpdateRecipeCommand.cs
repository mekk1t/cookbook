namespace KP.Cookbook.Features.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public long RecipeId { get; }
        public long? SourceId { get; }
        public int DurationMinutes { get; }
        public string? Description { get; }
        public string? ImageBase64 { get; }

        public UpdateRecipeCommand(
            long recipeId,
            long? sourceId = null,
            int durationMinutes = 0,
            string? description = null,
            string? imageBase64 = null)
        {
            RecipeId = recipeId;
            SourceId = sourceId;
            DurationMinutes = durationMinutes;
            Description = description;
            ImageBase64 = imageBase64;
        }
    }
}
