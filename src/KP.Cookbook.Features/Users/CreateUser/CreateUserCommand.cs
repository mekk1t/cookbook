﻿using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Users.CreateUser
{
    public class CreateUserCommand
    {
        public User NewUser { get; }

        public CreateUserCommand(User newUser)
        {
            NewUser = newUser;
        }
    }
}