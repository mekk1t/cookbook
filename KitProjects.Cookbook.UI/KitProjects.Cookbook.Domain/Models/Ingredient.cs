namespace KitProjects.Cookbook.Domain.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; set; }
        public IngredientType Type { get; set; }
    }
}
