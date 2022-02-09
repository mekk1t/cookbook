using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.UpdateIngredient
{
    public class UpdateIngredientCommandHandler : ICommandHandler<UpdateIngredientCommand>
    {
        private readonly IngredientsRepository _repository;

        public UpdateIngredientCommandHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(UpdateIngredientCommand command) => _repository.Update(command.NewIngredient);
    }
}
