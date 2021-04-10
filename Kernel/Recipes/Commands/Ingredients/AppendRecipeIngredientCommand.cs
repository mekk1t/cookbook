using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class AppendRecipeIngredientCommand
    {
        public Guid RecipeId { get; }
        public Guid IngredientId { get; }

        public AppendRecipeIngredientCommand(Guid recipeId, Guid ingredientId)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }
    }
}
