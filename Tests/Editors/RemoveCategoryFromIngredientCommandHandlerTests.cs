using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands.Edit.Ingredient;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using System;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class RemoveCategoryFromIngredientCommandHandlerTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly ICommand<RemoveIngredientCategoryCommand> _sut;

        public RemoveCategoryFromIngredientCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            _sut =
                new RemoveCategoryFromIngredientDecorator(
                    new CategoryChecker(
                        new GetCategoryQueryHandler(_dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(_dbContext)),
                    new RemoveIngredientCategoryCommandHandler(_dbContext));
        }

        [Fact]
        public void Editor_removes_category_from_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString())
            {
                Categories = { new Category(removeCategoryId, removeCategoryId.ToString()) }
            });

            Action act = () => _sut.Execute(new RemoveIngredientCategoryCommand(removeCategoryId.ToString(), ingredientId));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientId.ToString());
            result.Categories.Should().BeEmpty();
        }

        [Fact]
        public void Editor_proceeds_with_removing_nonexistent_category_from_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));

            Action act = () => _sut.Execute(new RemoveIngredientCategoryCommand(removeCategoryId.ToString(), ingredientId));

            act.Should().NotThrow();
        }

        [Fact]
        public void Editor_cant_remove_category_from_nonexistent_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(removeCategoryId, removeCategoryId.ToString()));

            Action act = () => _sut.Execute(new RemoveIngredientCategoryCommand(removeCategoryId.ToString(), ingredientId));

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_proceeds_with_removing_category_that_ingredient_does_not_have()
        {
            var ingredientId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            var otherCategoryId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString())
            {
                Categories = { new Category(removeCategoryId, removeCategoryId.ToString()) }
            });

            Action act = () => _sut.Execute(new RemoveIngredientCategoryCommand(otherCategoryId.ToString(), ingredientId));

            act.Should().NotThrow();
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
