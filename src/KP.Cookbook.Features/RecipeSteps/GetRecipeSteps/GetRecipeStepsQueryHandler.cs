using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.RecipeSteps.GetRecipeSteps
{
    public class GetRecipeStepsQueryHandler : IQueryHandler<GetRecipeStepsQuery, CookingStepsCollection>
    {
        private readonly RecipesRepository _recipesRepository;

        public GetRecipeStepsQueryHandler(RecipesRepository recipesRepository)
        {
            _recipesRepository = recipesRepository;
        }

        public CookingStepsCollection Execute(GetRecipeStepsQuery query) => _recipesRepository.GetRecipeSteps(query.RecipeId);
    }
}
