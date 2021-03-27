using DatabaseTestsConnector;
using FluentAssertions;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Moderators
{
    [Collection("Db")]
    public sealed class CategoryModeratorTests : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CategoryModerator _sut;
        private List<DbContext> _dbContexts;

        public CategoryModeratorTests(DbFixture fixture)
        {
            _fixture = fixture;
            var dbContext = _fixture.DbContext;
            _dbContexts = new List<DbContext>
            {
                dbContext
            };
            _sut = new CategoryModerator(new CreateCategoryCommandHandler(dbContext), new GetCategoriesQuery(dbContext));
        }

        [Fact]
        public void Category_moderator_creates_a_new_category()
        {
            Action act = () => _sut.CreateCategory(new CreateCategoryCommand("Тест"));

            act.Should().NotThrow();
            using var _dbContext = _fixture.DbContext;
            var result = _dbContext.Categories.First(r => r.Name == "Тест");
            result.Should().NotBeNull();
            result.Name.Should().Be("Тест");
        }

        [Fact]
        public void Cant_create_a_new_category_with_existing_name()
        {
            var categoryName = "бавгд";
            _fixture.SeedCategory(new Category(Guid.NewGuid(), categoryName));
            Action act = () => _sut.CreateCategory(new CreateCategoryCommand(categoryName));

            act.Should().NotThrow();
            using var _dbContext = _fixture.DbContext;
            _dbContext.Categories.Where(r => r.Name == categoryName).Should().HaveCount(1);
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
