using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand>
    {
        private readonly RecipesRepository _recipesRepository;
        private readonly SourcesRepository _sourcesRepository;

        public UpdateRecipeCommandHandler(RecipesRepository recipesRepository, SourcesRepository sourcesRepository)
        {
            _recipesRepository = recipesRepository;
            _sourcesRepository = sourcesRepository;
        }

        public void Execute(UpdateRecipeCommand command)
        {
            var recipe = _recipesRepository.GetRecipe(command.RecipeId);
            if (recipe == null)
                throw new Exception($"Рецепт с ID {command.RecipeId} не найден");

            Source? source = null;
            if (command.SourceId.HasValue)
                source = _sourcesRepository.GetById(command.SourceId.Value);

            recipe.Edit(source, command.DurationMinutes, command.Description, command.ImageBase64);

            _recipesRepository.Update(recipe);
        }
    }
}
