using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.Kernel.Models.Recipes
{
    public class StepIngredientDetails
    {
        public string IngredientName { get; init; }
        public decimal Amount { get; init; }
        public Measures Measure { get; init; }
    }
}
