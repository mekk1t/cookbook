using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Ingredients.CreateIngredient
{
    public class CreateIngredientCommandHandler : ICommandHandler<CreateIngredientCommand, Ingredient>
    {
        private readonly IngredientsRepository _repository;

        public CreateIngredientCommandHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public Ingredient Execute(CreateIngredientCommand command) => _repository.Create(command.Ingredient);
    }
}
