using FluentAssertions;
using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace KitProjects.Cookbook.Tests.IntegrationTests
{
    public class RepositoryTests : IDisposable
    {
        private readonly CookbookDbContext _dbContext;
        private readonly Repository<Recipe> _sut;

        public RepositoryTests()
        {
            _dbContext =
                new CookbookDbContext(
                    new DbContextOptionsBuilder<CookbookDbContext>()
                        .UseSqlServer("Server=localhost;Database=Cookbook_TESTS;Trusted_Connection=True;").Options);
            _dbContext.Database.EnsureCreated();
            _sut = new Repository<Recipe>(_dbContext);
        }

        [Fact]
        public void Можно_сохранить_рецепт_без_связанных_сущностей()
        {
            var recipe = new Recipe
            {
                Description = "Описание",
                Synopsis = "Синопсис",
                Title = "Название",
                Tags = new List<string> { "Тег1", "Тег2" },
                CookingDuration = 12,
            };

            _sut.Save(recipe);

            recipe.Id.Should().NotBe(default);
        }

        [Fact]
        public void Можно_сохранить_ингредиент()
        {
            var ingredient = new Ingredient
            {
                Name = "Огурец",
                Type = IngredientType.Овощи
            };
            var sut = new Repository<Ingredient>(_dbContext);

            sut.Save(ingredient);

            ingredient.Id.Should().NotBe(default);
        }

        [Fact]
        public void Можно_сохранить_рецепт_с_ингредиентами()
        {
            var ingredient = new Ingredient { Name = "Ингредиент" };
            new Repository<Ingredient>(_dbContext).Save(ingredient);
            _dbContext.ChangeTracker.Clear();
            var recipe = new Recipe
            {
                IngredientDetails = new List<IngredientDetails>
                {
                    new IngredientDetails
                    {
                        Ingredient = ingredient,
                        Amount = 12,
                        Measure = IngredientMeasure.гр,
                        Optional = false
                    }
                }
            };

            _sut.Save(recipe);

            recipe.Id.Should().NotBe(default);
            recipe.IngredientDetails[0].Ingredient.Id.Should().Be(ingredient.Id);
        }

        [Fact]
        public void Можно_сохранить_рецепт_с_шагами_с_ингредиентами()
        {
            var ingredient = new Ingredient { Name = "Ингредиент" };
            new Repository<Ingredient>(_dbContext).Save(ingredient);
            _dbContext.ChangeTracker.Clear();
            var recipe = new Recipe
            {
                IngredientDetails = new List<IngredientDetails>
                {
                    new IngredientDetails
                    {
                        Ingredient = ingredient,
                        Amount = 12,
                        Measure = IngredientMeasure.гр,
                        Optional = false
                    }
                },
                Steps = new List<Step>
                {
                    new Step
                    {
                        Description = "Описание",
                        Order = 1,
                        IngredientDetails = new List<IngredientDetails>
                        {
                            new IngredientDetails
                            {
                                Ingredient = ingredient,
                                Amount = 1,
                                Measure = IngredientMeasure.гр,
                                Optional = true
                            }
                        }
                    }
                }
            };

            _sut.Save(recipe);

            recipe.Id.Should().NotBe(default);
            ingredient.Id.Should().NotBe(default);
            recipe.IngredientDetails[0].Ingredient.Id.Should().Be(ingredient.Id);
            recipe.Steps[0].IngredientDetails[0].Ingredient.Id.Should().Be(ingredient.Id);
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
