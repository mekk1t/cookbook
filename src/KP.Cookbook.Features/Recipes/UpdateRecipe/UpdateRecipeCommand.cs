using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommand
    {
        public long RecipeId { get; }
        public Source? Source { get; }
        public int DurationMinutes { get; }
        public string? Description { get; }
        public string? ImageBase64 { get; }

        public UpdateRecipeCommand(
            long recipeId,
            Source? source = default,
            int durationMinutes = 0,
            string? description = null,
            string? imageBase64 = null)
        {
            RecipeId = recipeId;
            Source = source;
            DurationMinutes = durationMinutes;
            Description = description;
            ImageBase64 = imageBase64;
        }
    }
}
