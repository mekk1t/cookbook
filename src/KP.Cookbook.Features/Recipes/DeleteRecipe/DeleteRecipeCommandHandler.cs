using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Recipes.DeleteRecipe
{
    public class DeleteRecipeCommandHandler : ICommandHandler<DeleteRecipeCommand>
    {
        private readonly RecipesRepository _repository;

        public DeleteRecipeCommandHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public void Execute(DeleteRecipeCommand command) => _repository.DeleteById(command.RecipeId);
    }
}
