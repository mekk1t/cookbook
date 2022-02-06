using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.RestApi.Controllers.Ingredients.Requests
{
    public class CreateIngredientRequest
    {
        public string Name { get; set; } = string.Empty;
        public IngredientType Type { get; set; }
        public string? Description { get; set; }
    }
}
