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
    public class RemoveCategoryFromRecipeCommandHandlerTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly ICommand<RemoveRecipeCategoryCommand> _sut;

        public RemoveCategoryFromRecipeCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            _sut =
                new RemoveCategoryFromRecipeDecorator(
                    new CategoryChecker(
                        new GetCategoryQueryHandler(_dbContext)),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(_dbContext)),
                    new RemoveRecipeCategoryCommandHandler(_dbContext));
        }

        [Fact]
        public void Editor_proceeds_with_removing_nonexistent_category()
        {
            var categoryId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });

            Action act = () => _sut.Execute(new RemoveRecipeCategoryCommand(recipeId, categoryId.ToString()));

            act.Should().NotThrow();
        }

        [Fact]
        public void Editor_cant_remove_category_from_nonexistent_recipe()
        {
            var categoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            var recipeId = Guid.NewGuid();

            Action act = () => _sut.Execute(new RemoveRecipeCategoryCommand(recipeId, categoryId.ToString()));

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_proceeds_with_removing_category_which_recipe_does_not_have()
        {
            var categoryId = Guid.NewGuid();
            var otherCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            _fixture.SeedCategory(new Category(otherCategoryId, otherCategoryId.ToString()));
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

            Action act = () => _sut.Execute(new RemoveRecipeCategoryCommand(recipeId, otherCategoryId.ToString()));

            act.Should().NotThrow();
        }

        [Fact]
        public void Editor_removes_a_category_from_recipe()
        {
            var categoryId = Guid.NewGuid();
            var removeCategoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            _fixture.SeedCategory(new Category(removeCategoryId, removeCategoryId.ToString()));
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
                    },
                    new DbRecipeCategory
                    {
                        DbRecipeId = recipeId,
                        DbCategoryId = removeCategoryId
                    }
                }
            });
            _fixture.FindRecipe(recipeId).RecipeCategoriesLink.Select(link => link.DbCategoryId).Should().Contain(removeCategoryId);

            Action act = () => _sut.Execute(new RemoveRecipeCategoryCommand(recipeId, removeCategoryId.ToString()));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeCategoriesLink.Select(link => link.DbCategoryId).Should().NotContain(removeCategoryId);
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
