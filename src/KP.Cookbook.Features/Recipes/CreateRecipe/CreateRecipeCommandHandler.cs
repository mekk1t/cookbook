using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Database.Models;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, DbRecipe>
    {
        private readonly RecipesRepository _repository;
        private readonly UsersRepository _usersRepository;

        public CreateRecipeCommandHandler(RecipesRepository repository, UsersRepository usersRepository)
        {
            _repository = repository;
            _usersRepository = usersRepository;
        }

        public DbRecipe Execute(CreateRecipeCommand command)
        {
            var user = _usersRepository.GetByLoginOrDefault(command.UserLogin);
            if (user == null)
                throw new Exception($"Пользователь с логином {command.UserLogin} не найден.");

            var recipe = Recipe.Create(
                command.Title,
                user,
                command.RecipeType,
                command.CookingType,
                command.KitchenType,
                command.HolidayType);

            return _repository.Save(recipe);
        }
    }
}
