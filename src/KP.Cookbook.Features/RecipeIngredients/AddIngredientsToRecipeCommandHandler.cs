using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Database.Models;

namespace KP.Cookbook.Features.RecipeIngredients
{
    public class AddIngredientsToRecipeCommandHandler : ICommandHandler<AddIngredientsToRecipeCommand>
    {
        private readonly IngredientsRepository _ingredientsRepository;

        public AddIngredientsToRecipeCommandHandler(IngredientsRepository ingredientsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
        }

        public void Execute(AddIngredientsToRecipeCommand command) =>
            _ingredientsRepository.AddIngredientsToRecipe(command.RecipeId, command.Ingredients.Select(i => new DbIngredientDetailed
            {
                IngredientId = i.Id,
                Amount = i.Amount,
                IsOptional = i.IsOptional,
                AmountType = i.AmountType,
            }));
    }
}
