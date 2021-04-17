using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients
{
    public class EditRecipeIngredientDescriptionCommand
    {
        public Guid RecipeId { get; }
        public Guid IngredientId { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }
        public string Notes { get; }

        public EditRecipeIngredientDescriptionCommand(
            Guid recipeId, Guid ingredientId,
            decimal amount = 0M, Measures measure = Measures.None, string notes = "")
        {
            Amount = amount;
            Measure = measure;
            Notes = notes;
            RecipeId = recipeId;
            IngredientId = ingredientId;
        }

        public bool HasValues => Amount != default || Measure != Measures.None || Notes.IsNotNullOrEmpty();
    }
}
