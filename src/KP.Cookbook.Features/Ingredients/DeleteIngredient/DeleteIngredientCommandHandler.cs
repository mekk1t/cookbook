using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Ingredients.DeleteIngredient
{
    public class DeleteIngredientCommandHandler : ICommandHandler<DeleteIngredientCommand>
    {
        private readonly IngredientsRepository _repository;

        public DeleteIngredientCommandHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(DeleteIngredientCommand command) => _repository.Delete(command.Id);
    }
}
