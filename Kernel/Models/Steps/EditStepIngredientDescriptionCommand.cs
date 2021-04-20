using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class EditStepIngredientDescriptionCommand
    {
        public RecipeStepIds Ids { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }
        public Guid IngredientId { get; }

        public EditStepIngredientDescriptionCommand(
            RecipeStepIds ids,
            Guid ingredientId,
            decimal amount = 0, Measures measure = Measures.None)
        {
            Ids = ids;
            Amount = amount;
            Measure = measure;
            IngredientId = ingredientId;
        }
    }
}
