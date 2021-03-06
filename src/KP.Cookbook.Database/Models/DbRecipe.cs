using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database.Models
{
    public class DbRecipe
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public RecipeType RecipeType { get; set; }
        public CookingType CookingType { get; set; }
        public KitchenType KitchenType { get; set; }
        public HolidayType HolidayType { get; set; }
        public DateTime CreatedAt { get; set; }
        public long UserId { get; set; }
        public long? SourceId { get; set; }
        public int DurationMinutes { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
