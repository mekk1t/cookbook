using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.StepIngredients.RemoveIngredientFromStep
{
    public class RemoveIngredientFromStepCommandHandler : ICommandHandler<RemoveIngredientFromStepCommand>
    {
        private readonly CookingStepsRepository _repository;

        public RemoveIngredientFromStepCommandHandler(CookingStepsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(RemoveIngredientFromStepCommand command) =>
            _repository.RemoveIngredientFromStep(command.StepId, command.IngredientId);
    }
}
