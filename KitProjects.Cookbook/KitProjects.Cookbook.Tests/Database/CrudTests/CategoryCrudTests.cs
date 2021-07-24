﻿using FluentAssertions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database.Crud;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace KitProjects.Cookbook.Tests.Database.CrudTests
{
    [Collection(nameof(DbFixture))]
    public class CategoryCrudTests : DatabaseTests
    {
        private readonly CategoryCrud _sut;

        public CategoryCrudTests(DbFixture dbFixture) : base(dbFixture)
        {
            _sut = new CategoryCrud(_dbContext);
        }

        [Fact]
        public void Нельзя_создать_категорию_с_пустым_именем()
        {
            var newCategory = new Category
            {
                Type = CategoryType.Ingredient,
                Name = null
            };

            Action act = () => _sut.Create(newCategory);

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void Успешное_создание_валидной_категории_без_ингредиентов()
        {
            var newCategory = new Category
            {
                Type = CategoryType.Recipe,
                Name = "Heisenberg"
            };

            var result = _sut.Create(newCategory);

            result.Name.Should().Be("Heisenberg");
        }

        [Fact]
        public void Успешное_создание_валидной_категории_с_прежде_не_существовавшими_ингредиентами()
        {
            var newCategory = CreateCategoryWithIngredients("ТЕСТ", "ДРУГОЙ_ТЕСТ", "ИНГРЕДИЕНТИЩЕ");

            var result = _sut.Create(newCategory);

            result.Ingredients.Count.Should().Be(3);
        }

        [Fact]
        public void Успешное_создание_валидной_категории_с_уже_существующими_ингредиентами()
        {
            var existingIngredients = new[]
            {
                SeedIngredient("Хоух"),
                SeedIngredient("Пройдемте в дом")
            };
            var category = CreateCategoryWithIngredients();
            category.Ingredients = existingIngredients.ToList();

            var result = _sut.Create(category);

            result.Ingredients.Should().HaveCount(2);
        }

        [Fact]
        public void Успешное_создание_валидной_категории_с_существующими_и_новыми_ингредиентами()
        {
            var existingIngredients = new[]
            {
                SeedIngredient("НОВИНКА!"),
                SeedIngredient("НЕ ПОВТОРЯЙТЕ ЭТО ДОМА!")
            };
            var category = CreateCategoryWithIngredients("Мафака новенький ингредиентик");
            category.Ingredients.AddRange(existingIngredients);

            var result = _sut.Create(category);

            result.Ingredients.Should().HaveCount(3);
        }

        [Fact]
        public void Круд_удаляет_категорию_по_ID()
        {
            var existingCategory = SeedCategory("МАФАКА");

            Action act = () => _sut.Delete(existingCategory);

            act.Should().NotThrow();
        }

        private Ingredient SeedIngredient(string name)
        {
            var entity = new Ingredient { Name = name };
            _dbContext.Ingredients.Add(entity);
            _dbContext.SaveChanges();
            return new Ingredient(entity);
        }

        private Category SeedCategory(string name) =>
            _sut.Create(new Category { Name = name });

        private static Category CreateCategoryWithIngredients(params string[] ingredientNames) =>
            new()
            {
                Name = Guid.NewGuid().ToString(),
                Type = CategoryType.Recipe,
                Ingredients = ingredientNames.Select(name => new Ingredient
                {
                    Name = name
                }).ToList()
            };
    }
}
