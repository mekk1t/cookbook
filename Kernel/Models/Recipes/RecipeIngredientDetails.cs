using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeIngredientDetails
    {
        public string IngredientName { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }
        public string Notes { get; }

        public RecipeIngredientDetails(string ingredientName, Measures measure, decimal amount, string notes = "")
        {
            IngredientName = ingredientName;
            Amount = amount;
            Measure = measure;
            Notes = notes;
        }
    }
}
