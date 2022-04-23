using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.Recipes.GetRecipes;

namespace KP.Cookbook.Features.Recipes.GetRecipeDetails
{
    public class RecipeDetailsDto
    {
        public RecipeDto Recipe { get; }
        public User Author { get; }
        public Source? Source { get; }
        public IReadOnlyList<CookingStep> CookingSteps { get; }
        public List<IngredientDetailed> Ingredients { get; }

        public RecipeDetailsDto(
            RecipeDto recipe,
            User author,
            Source? source,
            CookingStepsCollection cookingSteps,
            List<IngredientDetailed> ingredients)
        {
            Recipe = recipe;
            Author = author;
            Source = source;
            CookingSteps = cookingSteps.Steps;
            Ingredients = ingredients;
        }
    }
}
