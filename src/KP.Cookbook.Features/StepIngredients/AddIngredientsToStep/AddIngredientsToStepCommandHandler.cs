using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.StepIngredients.AddIngredientsToStep
{
    public class AddIngredientsToStepCommandHandler : ICommandHandler<AddIngredientsToStepCommand>
    {
        private readonly IngredientsRepository _ingredientsRepository;
        private readonly CookingStepsRepository _cookingStepsRepository;

        public AddIngredientsToStepCommandHandler(IngredientsRepository ingredientsRepository, CookingStepsRepository cookingStepsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
            _cookingStepsRepository = cookingStepsRepository;
        }

        public void Execute(AddIngredientsToStepCommand command)
        {
            var recipeIngredientsDetailed = _ingredientsRepository.GetRecipeIngredients(command.RecipeId);
            var recipeIngredients = recipeIngredientsDetailed.Select(d => d.Ingredient).ToList();

            var newIngredients = _ingredientsRepository.Get(command.Ingredients.Select(i => i.Id));

            foreach (var newIngredient in newIngredients)
            {
                if (!recipeIngredients.Contains(newIngredient))
                {
                    throw new Exception(
                        $"Ингредиент {newIngredient.Name} отсутствует в рецепте. " +
                        $"Сначала необходимо добавить ингредиент в рецепт");
                }
            }

            var currentStepIngredientsDetailed = _cookingStepsRepository.GetStepIngredients(command.StepId);
            var stepIngredients = currentStepIngredientsDetailed.Select(si => si.Ingredient).ToList();
            foreach (var newIngredient in newIngredients)
            {
                if (stepIngredients.Contains(newIngredient))
                    throw new Exception($"В шаге уже есть ингредиент {newIngredient.Name}");
            }

            _cookingStepsRepository.AddIngredientsToStep(
                command.StepId,
                command.Ingredients.Select(i => new Database.Models.DbIngredientDetailed
                {
                    AmountType = i.AmountType,
                    Amount = i.Amount,
                    IsOptional = i.IsOptional,
                    IngredientId = i.Id
                }));
        }
    }
}
