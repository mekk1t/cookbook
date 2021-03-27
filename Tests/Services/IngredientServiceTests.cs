using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Services
{
    [Collection("Db")]
    public class IngredientServiceTests
    {
        private readonly DbFixture _fixture;
        private readonly List<DbContext> _dbContexts;
        private readonly IngredientService _sut;

        public IngredientServiceTests(DbFixture fixture)
        {
            _dbContexts = new List<DbContext>();
            _fixture = fixture;
            var dbContext = _fixture.DbContext;
            _dbContexts.Add(dbContext);
            _sut = new IngredientService(
                new CreateIngredientCommandHandler(dbContext),
                new GetIngredientsQuery(dbContext),
                new CategoryService(
                    new CreateCategoryCommandHandler(dbContext),
                    new GetCategoriesQuery(dbContext),
                    new DeleteCategoryCommandHandler(dbContext),
                    new EditCategoryCommandHandler(dbContext)));
        }

        [Fact]
        public void Can_create_an_ingredient_without_categories()
        {
            var ingredientName = "Ингредиент";

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new List<string>()));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Categories.Should().BeEmpty();
            result.Name.Should().Be(ingredientName);
        }

        [Fact]
        public void Can_create_an_ingredient_with_nonexistent_categories()
        {
            using var dbContext = _fixture.DbContext;
            var ingredientName = Guid.NewGuid().ToString();
            var newCategories = new List<string>()
            {
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()
            };

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, newCategories));

            act.Should().NotThrow();
            dbContext.Categories.AsNoTracking().Select(c => c.Name).Should().Contain(newCategories);
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Name.Should().Be(ingredientName);
            result.Categories.Select(c => c.Name).Should().Contain(newCategories);
        }

        [Fact]
        public void Can_create_an_ingredient_with_existing_categories()
        {
            string ingredientName = "Новый ингредиент";
            _fixture.SeedCategory(new Category(Guid.NewGuid(), "1"));
            _fixture.SeedCategory(new Category(Guid.NewGuid(), "2"));

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new[] { "1", "2" }));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Name.Should().Be(ingredientName);
            result.Categories.Select(c => c.Name).Should().Contain(new[] { "1", "2" });
        }

        [Fact]
        public void Cant_create_a_duplicate_name_ingredient()
        {
            string ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredient(new Ingredient(Guid.NewGuid(), ingredientName, new List<Category>()));

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new List<string>()));

            act.Should().NotThrow();
            using var dbContext = _fixture.DbContext;
            dbContext.Ingredients.Where(i => i.Name == ingredientName).Should().HaveCount(1);
        }
    }
}
