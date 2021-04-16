using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.WebApplication.RecipeSteps
{
    public class StepIngredientRequestDetails
    {
        public string IngredientName { get; set; }
        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
    }
}
