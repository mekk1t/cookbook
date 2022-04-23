using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Features.RecipeIngredients.GetRecipeIngredients
{
    public class GetRecipeIngredientsQueryHandler : IQueryHandler<GetRecipeIngredientsQuery, List<IngredientDetailed>>
    {
        private readonly IngredientsRepository _repository;

        public GetRecipeIngredientsQueryHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public List<IngredientDetailed> Execute(GetRecipeIngredientsQuery query) => _repository.GetRecipeIngredients(query.RecipeId);
    }
}
