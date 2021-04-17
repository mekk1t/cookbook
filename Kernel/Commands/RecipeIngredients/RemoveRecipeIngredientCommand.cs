using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class RemoveRecipeIngredientCommand
    {
        public Guid RecipeId { get; }
        public Guid IngredientId { get; }

        public RemoveRecipeIngredientCommand(Guid recipeId, Guid ingredientId)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }
    }
}
