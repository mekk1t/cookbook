using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands.Edit.Ingredient;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public class AppendCategoryToIngredientCommandHandlerTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly ICommand<AppendExistingCategoryToIngredientCommand> _sut;

        public AppendCategoryToIngredientCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            _sut = new AppendCategoryToIngredientDecorator(
                new CategoryChecker(
                    new GetCategoryQueryHandler(_dbContext)),
                new GetIngredientQueryHandler(_dbContext),
                new AppendExistingCategoryToIngredientCommandHandler(_dbContext));
        }

        [Fact]
        public void Editor_cant_append_same_category_twice()
        {
            var ingredientId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString())
            {
                Categories = { new Category(categoryId, categoryId.ToString()) }
            });

            Action act = () => _sut.Execute(new AppendExistingCategoryToIngredientCommand(categoryId.ToString(), ingredientId));

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Editor_appends_category_to_ingredient_with_categories()
        {
            var ingredientId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString())
            {
                Categories = { new Category(categoryId, categoryId.ToString()) }
            });

            Action act = () => _sut.Execute(new AppendExistingCategoryToIngredientCommand(appendCategoryId.ToString(), ingredientId));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientId.ToString());
            result.Categories.Select(c => c.Id).Should().Contain(appendCategoryId);
        }

        [Fact]
        public void Editor_appends_category_to_ingredient_without_categories()
        {
            var ingredientId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));

            Action act = () => _sut.Execute(new AppendExistingCategoryToIngredientCommand(appendCategoryId.ToString(), ingredientId));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientId.ToString());
            result.Categories.Select(c => c.Id).Should().Contain(appendCategoryId);
        }

        [Fact]
        public void Editor_cant_append_nonexistent_category_to_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));

            Action act = () => _sut.Execute(new AppendExistingCategoryToIngredientCommand(appendCategoryId.ToString(), ingredientId));

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_cant_append_category_to_nonexistent_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));

            Action act = () => _sut.Execute(new AppendExistingCategoryToIngredientCommand(appendCategoryId.ToString(), ingredientId));

            act.Should().Throw<Exception>();
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
