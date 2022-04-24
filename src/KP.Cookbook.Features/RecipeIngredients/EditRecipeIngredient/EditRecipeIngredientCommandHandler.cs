using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Database.Models;

namespace KP.Cookbook.Features.RecipeIngredients.EditRecipeIngredient
{
    public class EditRecipeIngredientCommandHandler : ICommandHandler<EditRecipeIngredientCommand>
    {
        private readonly IngredientsRepository _repository;

        public EditRecipeIngredientCommandHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public void Execute(EditRecipeIngredientCommand command) =>
            _repository.UpdateRecipeIngredient(command.RecipeId, new DbIngredientDetailed
            {
                IngredientId = command.Ingredient.Id,
                Amount = command.Ingredient.Amount,
                AmountType = command.Ingredient.AmountType,
                IsOptional = command.Ingredient.IsOptional
            });
    }
}
