using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class AppendIngredientToStepCommand
    {
        public RecipeStepIds Ids { get; }
        public Ingredient Ingredient { get; }
        public IngredientParameters Parameters { get; }

        public AppendIngredientToStepCommand(RecipeStepIds ids, Ingredient ingredient, IngredientParameters parameters)
        {
            Ids = ids;
            Ingredient = ingredient;
            Parameters = parameters;
        }
    }
}
