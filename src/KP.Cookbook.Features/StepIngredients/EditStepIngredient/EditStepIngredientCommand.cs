using KP.Cookbook.Features.RecipeIngredients.Dtos;

namespace KP.Cookbook.Features.StepIngredients.EditStepIngredient
{
    public class EditStepIngredientCommand
    {
        public long StepId { get; }
        public RecipeIngredientDto Ingredient { get; }

        public EditStepIngredientCommand(long stepId, RecipeIngredientDto ingredient)
        {
            StepId = stepId;
            Ingredient = ingredient;
        }
    }
}
