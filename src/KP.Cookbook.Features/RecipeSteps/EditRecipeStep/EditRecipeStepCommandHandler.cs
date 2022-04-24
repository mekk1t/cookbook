using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.RecipeSteps.EditRecipeStep
{
    public class EditRecipeStepCommandHandler : ICommandHandler<EditRecipeStepCommand>
    {
        private readonly CookingStepsRepository _repository;

        public EditRecipeStepCommandHandler(CookingStepsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(EditRecipeStepCommand command)
        {
            var step = _repository.GetById(command.StepId);

            _repository.Update(new CookingStep(step.Id, step.Order)
            {
                Description = command.Description,
                Image = command.Image
            });
        }
    }
}
