using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.Kernel.Commands.RecipeIngredients
{
    public class IngredientParameters
    {
        public decimal Amount { get; }
        public Measures Measure { get; }
        public string Notes { get; }

        public IngredientParameters(decimal amount, Measures measure, string notes = "")
        {
            Amount = amount;
            Measure = measure;
            Notes = notes;
        }
    }
}
