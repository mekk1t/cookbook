using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe
{
    public class AddIngredientsToRecipeCommand
    {
        public long RecipeId { get; }
        public IEnumerable<NewRecipeIngredientDto> Ingredients { get; }

        public AddIngredientsToRecipeCommand(long recipeId, IEnumerable<NewRecipeIngredientDto> ingredients)
        {
            RecipeId = recipeId;
            Ingredients = ingredients;
        }
    }
}
