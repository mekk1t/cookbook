using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Recipes.GetRecipes
{
    public class GetRecipesQueryHandler : IQueryHandler<GetRecipesQuery, List<RecipeDto>>
    {
        private readonly RecipesRepository _repository;

        public GetRecipesQueryHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public List<RecipeDto> Execute(GetRecipesQuery query) => _repository.GetRecipes().Select(r => new RecipeDto(r)).ToList();
    }
}
