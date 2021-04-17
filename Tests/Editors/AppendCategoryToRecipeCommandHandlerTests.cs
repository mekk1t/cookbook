using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class AppendCategoryToRecipeCommandHandlerTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly ICommand<AppendCategoryToRecipeCommand> _sut;

        public AppendCategoryToRecipeCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            _sut =
                new AppendCategoryToRecipeDecorator(
                    new CategoryChecker(
                        new GetCategoryQueryHandler(_dbContext)),
                    new GetRecipeQueryHandler(_dbContext),
                    new AppendCategoryCommandHandler(_dbContext));
        }

        [Fact]
        public void Editor_appends_a_category_to_recipe()
        {
            var categoryId = Guid.NewGuid();
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeCategoriesLink = new[]
                {
                    new DbRecipeCategory
                    {
                        DbRecipeId = recipeId,
                        DbCategoryId = categoryId
                    }
                }
            });

            Action act = () => _sut.Execute(new AppendCategoryToRecipeCommand(appendCategoryId.ToString(), recipeId));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeCategoriesLink.Select(link => link.DbCategoryId).Should().Contain(appendCategoryId);
        }

        [Fact]
        public void Editor_cant_append_same_category_twice()
        {
            var appendCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(appendCategoryId, appendCategoryId.ToString()));
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeCategoriesLink = new[]
                {
                    new DbRecipeCategory
                    {
                        DbRecipeId = recipeId,
                        DbCategoryId = appendCategoryId
                    }
                }
            });

            Action act = () => _sut.Execute(new AppendCategoryToRecipeCommand(appendCategoryId.ToString(), recipeId));

            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Editor_cant_append_nonexistent_category()
        {
            var categoryId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });

            Action act = () => _sut.Execute(new AppendCategoryToRecipeCommand(categoryId.ToString(), recipeId));

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Editor_appends_category_to_recipe_without_categories()
        {
            var categoryId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });

            Action act = () => _sut.Execute(new AppendCategoryToRecipeCommand(categoryId.ToString(), recipeId));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeCategoriesLink.Select(link => link.DbCategoryId).Should().Contain(categoryId);
        }

        [Fact]
        public void Editor_cant_append_category_to_nonexistent_recipe()
        {
            var categoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            var recipeId = Guid.NewGuid();

            Action act = () => _sut.Execute(new AppendCategoryToRecipeCommand(categoryId.ToString(), recipeId));

            act.Should().Throw<ArgumentException>();
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
