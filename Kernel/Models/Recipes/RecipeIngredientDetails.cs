using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeIngredientDetails
    {
        public Guid IngredientId { get; }
        public string IngredientName { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }
        public string Notes { get; }

        public RecipeIngredientDetails(string ingredientName, Measures measure, decimal amount, string notes = "", Guid ingredientId = default)
        {
            IngredientName = ingredientName;
            Amount = amount;
            Measure = measure;
            Notes = notes;
            IngredientId = ingredientId;
        }
    }
}
