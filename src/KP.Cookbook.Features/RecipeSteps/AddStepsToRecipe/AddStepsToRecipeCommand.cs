using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.RecipeSteps.AddStepsToRecipe
{
    /// <summary>
    /// Команда на добавление шагов в рецепт.
    /// </summary>
    /// <remarks>
    /// Добавлять можно только новые шаги: если шагов в рецепте ещё нет или по порядку они идут после последнего шага.
    /// </remarks>
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
