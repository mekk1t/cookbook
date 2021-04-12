using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class AppendRecipeIngredientCommand
    {
        public Guid RecipeId { get; }
        public Guid IngredientId { get; }
        public AppendIngredientParameters Parameters { get; }

        public AppendRecipeIngredientCommand(Guid recipeId, Guid ingredientId, AppendIngredientParameters parameters)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
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
