using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand>
    {
        private readonly RecipesRepository _recipesRepository;

        public UpdateRecipeCommandHandler(RecipesRepository recipesRepository)
        {
            _recipesRepository = recipesRepository;
        }

        public void Execute(UpdateRecipeCommand command)
        {
            var recipe = _recipesRepository.GetRecipe(command.RecipeId);
            if (recipe == null)
                throw new Exception($"Рецепт с ID {command.RecipeId} не найден");

            recipe.Edit(command.Source, command.DurationMinutes, command.Description, command.ImageBase64);

            _recipesRepository.Update(recipe);
        }
    }
}
