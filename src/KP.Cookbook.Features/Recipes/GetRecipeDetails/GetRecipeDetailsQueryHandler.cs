using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.GetRecipeDetails
{
    public class GetRecipeDetailsQueryHandler : IQueryHandler<GetRecipeDetailsQuery, Recipe>
    {
        private readonly RecipesRepository _repository;

        public GetRecipeDetailsQueryHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public Recipe Execute(GetRecipeDetailsQuery query) => _repository.GetRecipe(query.RecipeId);
    }
}
