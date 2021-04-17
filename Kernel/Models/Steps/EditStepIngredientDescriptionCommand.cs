using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class EditStepIngredientDescriptionCommand
    {
        public RecipeStepIds Ids { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }

        public EditStepIngredientDescriptionCommand(RecipeStepIds ids, decimal amount = 0, Measures measure = Measures.None)
        {
            Ids = ids;
            Amount = amount;
            Measure = measure;
        }
    }
}
