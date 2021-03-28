using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.Kernel.Models.Recipes
{
    public class StepIngredientDetails
    {
        public string IngredientName { get; set; }
        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
    }
}
