using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class AppendIngredientRequest
    {
        public string IngredientName { get; set; }
        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
        public string Notes { get; set; }
    }
}
