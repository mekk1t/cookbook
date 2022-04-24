using KP.Cookbook.Features.RecipeIngredients.Dtos;

namespace KP.Cookbook.Features.RecipeIngredients.EditRecipeIngredient
{
    public class EditRecipeIngredientCommand
    {
        public long RecipeId { get; }
        public RecipeIngredientDto Ingredient { get; }

        public EditRecipeIngredientCommand(long recipeId, RecipeIngredientDto ingredient)
        {
            RecipeId = recipeId;
            Ingredient = ingredient;
        }
    }
}
