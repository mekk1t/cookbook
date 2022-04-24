using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.StepIngredients.EditStepIngredient
{
    public class EditStepIngredientCommandHandler : ICommandHandler<EditStepIngredientCommand>
    {
        private readonly CookingStepsRepository _repository;

        public EditStepIngredientCommandHandler(CookingStepsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(EditStepIngredientCommand command) =>
            _repository.UpdateStepIngredient(command.StepId, new Database.Models.DbIngredientDetailed
            {
                IngredientId = command.Ingredient.Id,
                Amount = command.Ingredient.Amount,
                AmountType = command.Ingredient.AmountType,
                IsOptional = command.Ingredient.IsOptional
            });
    }
}
