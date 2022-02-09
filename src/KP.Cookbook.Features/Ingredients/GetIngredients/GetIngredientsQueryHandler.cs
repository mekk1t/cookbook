using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Ingredients.GetIngredients
{
    public class GetIngredientsQueryHandler : IQueryHandler<GetIngredientsQuery, List<Ingredient>>
    {
        private readonly IngredientsRepository _repository;

        public GetIngredientsQueryHandler(IngredientsRepository repository)
        {
            _repository = repository;
        }

        public List<Ingredient> Execute(GetIngredientsQuery query) => _repository.Get();
    }
}
