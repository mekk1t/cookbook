using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class ReplaceIngredientsListCommand
    {
        public Ingredient[] OldIngredients { get; }
        public Ingredient[] NewIngredients { get; }
        public Guid RecipeId { get; }

        public ReplaceIngredientsListCommand(Ingredient[] oldIngredients, Ingredient[] newIngredients, Guid recipeId)
        {
            RecipeId = recipeId;
            OldIngredients = oldIngredients;
            NewIngredients = newIngredients;
        }
    }
}
