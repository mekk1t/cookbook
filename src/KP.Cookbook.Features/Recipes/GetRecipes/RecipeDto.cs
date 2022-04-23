using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.GetRecipes
{
    public class RecipeDto
    {
        public long Id { get; }
        public string Title { get; }
        public RecipeType RecipeType { get; }
        public CookingType CookingType { get; }
        public KitchenType KitchenType { get; }
        public HolidayType HolidayType { get; }
        public DateTime CreatedAt { get; }
        public int DurationMinutes { get; }
        public string? Description { get; }
        public string? Image { get; }
        public DateTime? UpdatedAt { get; }

        public RecipeDto(Recipe recipe)
        {
            Id = recipe.Id;
            Title = recipe.Title;
            RecipeType = recipe.RecipeType;
            CookingType = recipe.CookingType;
            KitchenType = recipe.KitchenType;
            HolidayType = recipe.HolidayType;
            CreatedAt = recipe.CreatedAt;
            DurationMinutes = recipe.DurationMinutes;
            Description = recipe.Description;
            Image = recipe.Image;
            UpdatedAt = recipe.UpdatedAt;
        }
    }
}
