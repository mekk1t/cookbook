using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Abstractions;

namespace KP.Cookbook.Features.Users.GetUsers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<User>>
    {
        private readonly UsersRepository _repository;
        private readonly IAccessValidator _accessValidator;

        public GetUsersQueryHandler(UsersRepository repository, IAccessValidator accessValidator)
        {
            _repository = repository;
            _accessValidator = accessValidator;
        }

        public List<User> Execute(GetUsersQuery query)
        {
            bool userIsNotAdmin = _accessValidator.IsCurrentUserOfType(UserType.Admin) == false;
            if (userIsNotAdmin)
                throw new InvariantException("Список пользователей доступен только администраторам");

            return _repository.GetAll();
        }
    }
}
