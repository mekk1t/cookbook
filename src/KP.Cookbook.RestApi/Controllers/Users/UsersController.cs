using KitProjects.Api.AspNetCore;
using KP.Cookbook.Cqrs;
using KP.Cookbook.Features.Users.CreateUser;
using KP.Cookbook.Features.Users.GetUsers;
using KP.Cookbook.RestApi.Controllers.Users.Requests;
using KP.Cookbook.RestApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DomainUser = KP.Cookbook.Domain.Entities.User;

namespace KP.Cookbook.RestApi.Controllers.Users
{
    public class UsersController : ApiJsonController
    {
        private readonly ICommandHandler<CreateUserCommand, DomainUser> _createUser;
        private readonly IQueryHandler<GetUsersQuery, List<DomainUser>> _getUsers;

        public UsersController(
            ICommandHandler<CreateUserCommand, DomainUser> createUser,
            IQueryHandler<GetUsersQuery, List<DomainUser>> getUsers,
            ILogger<UsersController> logger) : base(logger)
        {
            _createUser = createUser;
            _getUsers = getUsers;
        }

        /// <summary>
        /// Список пользователей в системе.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers() =>
            ExecuteCollectionRequest(() => _getUsers.Execute(new GetUsersQuery()));

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="request">Запрос на регистрацию.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register([FromBody] RegisterUserRequest request) =>
            ExecuteObjectRequest(() => _createUser.Execute(
                new CreateUserCommand(
                    DomainUser.Register(request.Email, request.Password.Sha256Hash(), request.Nickname ?? string.Empty))));
    }
}
