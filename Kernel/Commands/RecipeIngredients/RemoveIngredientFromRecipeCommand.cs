using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class RemoveIngredientFromRecipeCommand
    {
        public Guid RecipeId { get; }
        public Guid IngredientId { get; }

        public RemoveIngredientFromRecipeCommand(Guid recipeId, Guid ingredientId)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }
    }
}
