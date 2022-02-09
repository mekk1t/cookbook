using KP.Cookbook.Cqrs;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Features.Users.CreateUser;
using KP.Cookbook.Features.Users.GetUsers;
using KP.Cookbook.RestApi.Controllers.Users.Requests;
using KP.Cookbook.RestApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DomainUser = KP.Cookbook.Domain.Entities.User;

namespace KP.Cookbook.RestApi.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandHandler<CreateUserCommand, DomainUser> _createUser;
        private readonly IQueryHandler<GetUsersQuery, List<DomainUser>> _getUsers;

        public UsersController(
            ICommandHandler<CreateUserCommand, DomainUser> createUser,
            IQueryHandler<GetUsersQuery, List<DomainUser>> getUsers)
        {
            _createUser = createUser;
            _getUsers = getUsers;
        }

        [HttpGet]
        public List<DomainUser> GetUsers() => _getUsers.Execute(new GetUsersQuery());

        [HttpPost]
        public DomainUser Register([FromBody] RegisterUserRequest request) =>
            _createUser.Execute(new CreateUserCommand(DomainUser.Register(request.Email, request.Password.Sha256Hash())));
    }
}
