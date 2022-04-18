using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.GetRecipes
{
    public class GetRecipesQueryHandler : IQueryHandler<GetRecipesQuery, List<Recipe>>
    {
        private readonly RecipesRepository _repository;

        public GetRecipesQueryHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public List<Recipe> Execute(GetRecipesQuery query) => _repository.GetRecipes();
    }
}
