using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.RecipeSteps.AddStepsToRecipe
{
    public class AddStepsToRecipeCommandHandler : ICommandHandler<AddStepsToRecipeCommand>
    {
        private readonly RecipesRepository _recipesRepository;

        public AddStepsToRecipeCommandHandler(RecipesRepository recipesRepository)
        {
            _recipesRepository = recipesRepository;
        }

        public void Execute(AddStepsToRecipeCommand command)
        {
            var currentStepsCollection = _recipesRepository.GetRecipeSteps(command.RecipeId);
            int maxOrder = currentStepsCollection.Steps.Select(s => s.Order).Max();
            bool allNewStepsAreAfterLatestStep = command.CookingStepsCollection.Steps.All(step => step.Order > maxOrder);

            if (currentStepsCollection.IsEmpty || command.CookingStepsCollection.IsEmpty || allNewStepsAreAfterLatestStep)
            {
                _recipesRepository.AddStepsToRecipe(command.RecipeId, command.CookingStepsCollection);
                return;
            }

            var stepsOrders = command.CookingStepsCollection.Steps.Select(s => s.Order);
            foreach (var step in currentStepsCollection.Steps)
            {
                if (stepsOrders.Contains(step.Order))
                    throw new Exception($"Шаг с номером {step.Order} уже есть в рецепте.");
            }
        }
    }
}
