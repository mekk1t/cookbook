using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.RecipeIngredients.RemoveIngredientFromRecipe
{
    public class RemoveIngredientFromRecipeCommandHandler : ICommandHandler<RemoveIngredientFromRecipeCommand>
    {
        private readonly IngredientsRepository _ingredientsRepository;

        public RemoveIngredientFromRecipeCommandHandler(IngredientsRepository ingredientsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
        }

        public void Execute(RemoveIngredientFromRecipeCommand command) =>
            _ingredientsRepository.RemoveIngredientFromRecipe(command.RecipeId, command.IngredientId);
    }
}
