using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class RecipeEditorTests : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly RecipeEditor _sut;
        private readonly List<DbContext> _dbContexts = new List<DbContext>();

        public RecipeEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
            var appendDbContext = _fixture.DbContext;
            var removeDbContext = _fixture.DbContext;
            var queryDbContext = _fixture.DbContext;
            _sut = new RecipeEditor(
                new AppendCategoryCommandHandler(appendDbContext),
                new RemoveRecipeCategoryCommandHandler(removeDbContext),
                new SearchCategoryQueryHandler(queryDbContext),
                new SearchRecipeQueryHandler(queryDbContext));

            _dbContexts.AddRange(new[] { appendDbContext, removeDbContext, queryDbContext });
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

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), recipeId);

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

            Action act = () => _sut.AppendCategory(appendCategoryId.ToString(), recipeId);

            act.Should().ThrowExactly<ArgumentException>();
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

            Action act = () => _sut.RemoveCategory(removeCategoryId.ToString(), recipeId);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeCategoriesLink.Select(link => link.DbCategoryId).Should().NotContain(removeCategoryId);
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

            Action act = () => _sut.AppendCategory(categoryId.ToString(), recipeId);

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

            Action act = () => _sut.AppendCategory(categoryId.ToString(), recipeId);

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

            Action act = () => _sut.AppendCategory(categoryId.ToString(), recipeId);

            act.Should().Throw<ArgumentException>();
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

            Action act = () => _sut.RemoveCategory(categoryId.ToString(), recipeId);

            act.Should().NotThrow();
        }

        [Fact]
        public void Editor_cant_remove_category_from_nonexistent_recipe()
        {
            var categoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            var recipeId = Guid.NewGuid();

            Action act = () => _sut.RemoveCategory(categoryId.ToString(), recipeId);

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

            Action act = () => _sut.RemoveCategory(otherCategoryId.ToString(), recipeId);

            act.Should().NotThrow();
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
