using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.RecipeIngredients.Dtos;

namespace KP.Cookbook.Features.RecipeIngredients.AddIngredientsToRecipe
{
    public class AddIngredientsToRecipeCommand
    {
        public long RecipeId { get; }
        public IEnumerable<RecipeIngredientDto> Ingredients { get; }

        public AddIngredientsToRecipeCommand(long recipeId, IEnumerable<RecipeIngredientDto> ingredients)
        {
            RecipeId = recipeId;
            Ingredients = ingredients;
        }
    }
}
