using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, Recipe>
    {
        private readonly RecipesRepository _repository;
        private readonly UsersRepository _usersRepository;

        public CreateRecipeCommandHandler(RecipesRepository repository, UsersRepository usersRepository)
        {
            _repository = repository;
            _usersRepository = _usersRepository;
        }

        public Recipe Execute(CreateRecipeCommand command)
        {
            var user = _usersRepository.GetByLoginOrDefault(command.UserLogin);
        }
            _repository.Save(
                Recipe.Create(
                    command.Title,
                    User.Create(command.UserLogin, command.UserNickname),
                    command.RecipeType,
                    command.CookingType,
                    command.KitchenType,
                    command.HolidayType));
    }
}
