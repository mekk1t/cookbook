using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.RecipeSteps.DeleteRecipeStep
{
    public class DeleteRecipeStepCommandHandler : ICommandHandler<DeleteRecipeStepCommand>
    {
        private readonly CookingStepsRepository _cookingStepsRepository;
        private readonly RecipesRepository _recipesRepository;

        public DeleteRecipeStepCommandHandler(CookingStepsRepository cookingStepsRepository, RecipesRepository recipesRepository)
        {
            _cookingStepsRepository = cookingStepsRepository;
            _recipesRepository = recipesRepository;
        }

        public void Execute(DeleteRecipeStepCommand command)
        {
            _recipesRepository.RemoveStepFromRecipe(command.RecipeId, command.StepId);
            _cookingStepsRepository.Delete(command.StepId);

            var currentSteps = _recipesRepository.GetRecipeSteps(command.RecipeId);
            currentSteps.NormalizeOrder();

            foreach (var step in currentSteps.Steps)
                _cookingStepsRepository.Update(step);
        }
    }
}
