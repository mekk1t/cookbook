using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;
using KP.Cookbook.Features.Recipes.GetRecipes;

namespace KP.Cookbook.Features.Recipes.GetRecipeDetails
{
    public class GetRecipeDetailsQueryHandler : IQueryHandler<GetRecipeDetailsQuery, RecipeDetailsDto>
    {
        private readonly RecipesRepository _repository;
        private readonly IngredientsRepository _ingredientsRepository;

        public GetRecipeDetailsQueryHandler(RecipesRepository repository, IngredientsRepository ingredientsRepository)
        {
            _repository = repository;
            _ingredientsRepository = ingredientsRepository;
        }

        public RecipeDetailsDto Execute(GetRecipeDetailsQuery query)
        {
            var recipe = _repository.GetRecipe(query.RecipeId);
            var steps = new CookingStepsCollection(Enumerable.Empty<CookingStep>());
            var ingredients = new List<IngredientDetailed>(0); // заменить на _ingredientsRepository.GetRecipeIngredients после реализации их работы с рецептом

            return new RecipeDetailsDto(new RecipeDto(recipe), recipe.Author, recipe.Source, steps, ingredients);
        }
    }
}
