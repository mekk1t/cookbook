using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Users.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
    {
        private readonly UsersRepository _repository;

        public CreateUserCommandHandler(UsersRepository repository)
        {
            _repository = repository;
        }

        public User Execute(CreateUserCommand command)
        {
            bool loginIsInUse = _repository.GetByLoginOrDefault(command.NewUser.Login) != null;
            if (loginIsInUse)
                throw new InvariantException("Пользователь с таким логином уже зарегистрирован");

            return _repository.Create(command.NewUser);
        }
    }
}
