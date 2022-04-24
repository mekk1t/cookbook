using KP.Cookbook.Features.RecipeIngredients.Dtos;

namespace KP.Cookbook.Features.StepIngredients.AddIngredientsToStep
{
    public class AddIngredientsToStepCommand
    {
        public long RecipeId { get; }
        public long StepId { get; }
        public List<RecipeIngredientDto> Ingredients { get; }

        public AddIngredientsToStepCommand(long recipeId, long stepId, List<RecipeIngredientDto> ingredients)
        {
            RecipeId = recipeId;
            StepId = stepId;
            Ingredients = ingredients;
        }
    }
}
