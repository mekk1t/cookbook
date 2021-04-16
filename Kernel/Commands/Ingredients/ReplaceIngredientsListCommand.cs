using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class ReplaceIngredientsListCommand
    {
        public Ingredient[] NewIngredients { get; }
        public Guid RecipeId { get; }

        public ReplaceIngredientsListCommand(Ingredient[] newIngredients, Guid recipeId)
        {
            RecipeId = recipeId;
            NewIngredients = newIngredients;
        }
    }
}
