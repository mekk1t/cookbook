using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class ReplaceRecipeIngredientCommand
    {
        public Ingredient OldIngredient { get; }
        public Ingredient NewIngredient { get; }
        public Guid RecipeId { get; }

        public ReplaceRecipeIngredientCommand(Ingredient oldIngredient, Ingredient newIngredient, Guid recipeId)
        {
            RecipeId = recipeId;
            OldIngredient = oldIngredient;
            NewIngredient = newIngredient;
        }
    }
}
