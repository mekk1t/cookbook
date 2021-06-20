using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class ReplaceRecipeIngredientsListCommand
    {
        public Ingredient[] NewIngredients { get; }
        public Guid RecipeId { get; }

        public ReplaceRecipeIngredientsListCommand(Ingredient[] newIngredients, Guid recipeId)
        {
            RecipeId = recipeId;
            NewIngredients = newIngredients;
        }
    }
}
