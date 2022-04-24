using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Features.StepIngredients.GetStepIngredients
{
    public class GetStepIngredientsQueryHandler : IQueryHandler<GetStepIngredientsQuery, List<IngredientDetailed>>
    {
        private readonly CookingStepsRepository _repository;

        public GetStepIngredientsQueryHandler(CookingStepsRepository repository)
        {
            _repository = repository;
        }

        public List<IngredientDetailed> Execute(GetStepIngredientsQuery query) => _repository.GetStepIngredients(query.StepId);
    }
}
