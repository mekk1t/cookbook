using KitProjects.MasterChef.Kernel.Models.Recipes;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands.Steps
{
    public class ReplaceStepCommand
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }
        public string Description { get; }
        public string Image { get; }
        public List<StepIngredientDetails> Ingredients { get; }

        public ReplaceStepCommand(Guid recipeId, Guid stepId, string description, string image, List<StepIngredientDetails> ingredients)
        {
            RecipeId = recipeId;
            StepId = stepId;
            Description = description;
            Image = image;
            Ingredients = ingredients;
        }
    }
}
