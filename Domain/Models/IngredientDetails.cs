namespace KitProjects.Cookbook.Domain.Models
{
    public class IngredientDetails
    {
        public Ingredient Ingredient { get; set; }
        public decimal Amount { get; set; }
        public IngredientMeasure Measure { get; set; }
        public bool Optional { get; set; }
    }
}
