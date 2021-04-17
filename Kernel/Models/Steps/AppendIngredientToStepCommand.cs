namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class AppendIngredientToStepCommand
    {
        public RecipeStepIds Ids { get; }
        public Ingredient Ingredient { get; }

        public AppendIngredientToStepCommand(RecipeStepIds ids, Ingredient ingredient)
        {
            Ids = ids;
            Ingredient = ingredient;
        }
    }
}
