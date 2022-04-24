using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.RecipeSteps.AddStepsToRecipe
{
    public class AddStepsToRecipeCommand
    {
        public long RecipeId { get; }
        public CookingStepsCollection CookingStepsCollection { get; }

        public AddStepsToRecipeCommand(long recipeId, CookingStepsCollection cookingStepsCollection)
        {
            RecipeId = recipeId;
            CookingStepsCollection = cookingStepsCollection;
        }
    }
}
