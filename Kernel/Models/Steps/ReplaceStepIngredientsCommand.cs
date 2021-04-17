namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class ReplaceStepIngredientsCommand
    {
        public RecipeStepIds Ids { get; }
        public Ingredient[] Ingredients { get; }

        public ReplaceStepIngredientsCommand(RecipeStepIds ids, Ingredient[] ingredients)
        {
            Ids = ids;
            Ingredients = ingredients;
        }
    }
}
