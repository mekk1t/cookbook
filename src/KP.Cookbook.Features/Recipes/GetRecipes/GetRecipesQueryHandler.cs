using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Database.Models;

namespace KP.Cookbook.Features.Recipes.GetRecipes
{
    public class GetRecipesQueryHandler : IQueryHandler<GetRecipesQuery, List<DbRecipe>>
    {
        private readonly RecipesRepository _repository;

        public GetRecipesQueryHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public List<DbRecipe> Execute(GetRecipesQuery query) => _repository.GetRecipes();
    }
}
