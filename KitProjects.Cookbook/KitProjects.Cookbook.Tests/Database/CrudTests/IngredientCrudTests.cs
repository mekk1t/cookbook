using FluentAssertions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Database.Crud;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace KitProjects.Cookbook.Tests.Database.CrudTests
{
    [Collection(nameof(DbFixture))]
    public class IngredientCrudTests : DatabaseTests
    {
        private readonly IngredientCrud _sut;

        public IngredientCrudTests(DbFixture fixture) : base(fixture)
        {
            _sut = new IngredientCrud(_dbContext);
        }

        [Fact]
        public void Нельзя_создать_ингредиент_без_названия()
        {
            var ingredient = new Ingredient() { Name = null };

            Action act = () => _sut.Create(ingredient);

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void Можно_создать_ингредиент_без_категорий()
        {
            var ingredient = new Ingredient { Name = "НАЗВАНИЕ!" };

            var result = _sut.Create(ingredient);

            result.Id.Should().NotBe(default);
        }

        [Fact]
        public void Можно_создать_ингредиент_с_новыми_и_старыми_категориями()
        {
            var existingCategory = SeedCategory("МафакаБич");
            var ingredient = Ingredient("ЙоуЙоуЙоу", existingCategory, new Category { Name = "SomeNewName" });

            var result = _sut.Create(ingredient);

            result.Categories.Should().HaveCount(2);
        }

        [Fact]
        public void Круд_Удаляет_ингредиент_по_ID()
        {
            var ingredient = _sut.Create(Ingredient("Mafakaaksaskakss"));

            Action act = () => _sut.Delete(ingredient);

            act.Should().NotThrow();
        }

        [Fact]
        public void Нельзя_удалить_несуществующий_ингредиент()
        {
            var nonExistentIngredient = new Ingredient(long.MaxValue);

            Action act = () => _sut.Delete(nonExistentIngredient);

            act.Should().Throw<EntityNotFoundException>();
        }

        [Fact]
        public void Редактирование_ингредиента_меняет_только_имя()
        {
            var oldIngredient = _sut.Create(Ingredient(Guid.NewGuid().ToString()));
            var update = new Ingredient(oldIngredient);
            update.Categories.Add(SeedCategory(Guid.NewGuid().ToString()));

            var result = _sut.Update(update);

            _sut.Read(update.Id).Categories.Should().BeEmpty();
        }

        private Ingredient Ingredient(string name, params Category[] categories)
        {
            var ingredient = new Ingredient { Name = name };
            ingredient.Categories.AddRange(categories);
            return ingredient;
        }

        private Category SeedCategory(string name)
        {
            var dbCategory = new Category { Name = name };
            _dbContext.Categories.Add(dbCategory);
            _dbContext.SaveChanges();
            return new Category(dbCategory);
        }
    }
}
