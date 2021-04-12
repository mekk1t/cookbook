using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class AppendRecipeIngredientCommand
    {
        public Guid RecipeId { get; }
        public Ingredient Ingredient { get; }
        public AppendIngredientParameters Parameters { get; }

        public AppendRecipeIngredientCommand(Guid recipeId, Ingredient ingredient, AppendIngredientParameters parameters)
        {
            RecipeId = recipeId;
            Ingredient = ingredient;
            Parameters = parameters;
        }
    }

    public class AppendIngredientParameters
    {
        public decimal Amount { get; }
        public Measures Measure { get; }
        public string Notes { get; }

        public AppendIngredientParameters(decimal amount, Measures measure, string notes = "")
        {
            Amount = amount;
            Measure = measure;
            Notes = notes;
        }
    }
}
