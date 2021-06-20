using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Models;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class AppendIngredientToRecipeCommand
    {
        public Guid RecipeId { get; }
        public Ingredient Ingredient { get; }
        public IngredientParameters Parameters { get; }

        public AppendIngredientToRecipeCommand(Guid recipeId, Ingredient ingredient, IngredientParameters parameters)
        {
            RecipeId = recipeId;
            Ingredient = ingredient;
            Parameters = parameters;
        }
    }
}
