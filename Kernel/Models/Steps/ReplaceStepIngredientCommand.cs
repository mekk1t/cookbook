namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class ReplaceStepIngredientCommand
    {
        public RecipeStepIds Ids { get; }
        public Ingredient OldIngredient { get; }
        public Ingredient NewIngredient { get; }

        public ReplaceStepIngredientCommand(RecipeStepIds ids, Ingredient oldIngredient, Ingredient newIngredient)
        {
            Ids = ids;
            OldIngredient = oldIngredient;
            NewIngredient = newIngredient;
        }
    }
}
