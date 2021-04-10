using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Edit.Ingredient;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class IngredientEditorTests : IDisposable
    {
        private readonly List<DbContext> _dbContexts = new();
        private readonly IngredientEditor _sut;
        private readonly DbFixture _fixture;

        public IngredientEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
            var queryDbContext = _fixture.DbContext;
            var appendDbContext = _fixture.DbContext;
            var removeDbContext = _fixture.DbContext;
            _sut = new IngredientEditor(
                new SearchCategoryQueryHandler(queryDbContext),
                new SearchIngredientQueryHandler(queryDbContext),
                new AppendIngredientCategoryCommandHandler(appendDbContext),
                new RemoveIngredientCategoryCommandHandler(removeDbContext));
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

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), ingredientId);

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

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), ingredientId);

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

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), ingredientId);

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_cant_append_category_to_nonexistent_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), ingredientId);

            act.Should().Throw<Exception>();
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

            Action act = () => _sut.RemoveCategory(removeCategoryId.ToString(), ingredientId);

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

            Action act = () => _sut.RemoveCategory(removeCategoryId.ToString(), ingredientId);

            act.Should().NotThrow();
        }

        [Fact]
        public void Editor_cant_remove_category_from_nonexistent_ingredient()
        {
            var ingredientId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(removeCategoryId, removeCategoryId.ToString()));

            Action act = () => _sut.RemoveCategory(removeCategoryId.ToString(), ingredientId);

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

            Action act = () => _sut.RemoveCategory(otherCategoryId.ToString(), ingredientId);

            act.Should().NotThrow();
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

            Action act = () => _sut.AppendCategory(categoryId.ToString(), ingredientId);

            act.Should().ThrowExactly<ArgumentException>();
        }

        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
