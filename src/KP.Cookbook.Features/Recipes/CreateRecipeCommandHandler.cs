﻿using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, Recipe>
    {
        private readonly RecipesRepository _repository;

        public CreateRecipeCommandHandler(RecipesRepository repository)
        {
            _repository = repository;
        }

        public Recipe Execute(CreateRecipeCommand command) =>
            _repository.Create(
                new Recipe(
                    command.Title,
                    User.Create(command.UserLogin, command.UserNickname),
                    command.RecipeType,
                    command.CookingType,
                    command.KitchenType,
                    command.HolidayType));
    }
}
