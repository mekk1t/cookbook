using DatabaseTestsConnector;
using FluentAssertions;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using Xunit;

namespace KitProjects.MasterChef.Tests.Moderators
{
    [Collection("Db")]
    public class CategoryModeratorTests
    {
        private readonly DbFixture _fixture;

        public CategoryModeratorTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Category_moderator_creates_a_new_category()
        {
            using var dbContext = _fixture.DbContext;
            var sut = new CategoryModerator(new CreateCategoryCommandHandler(dbContext), new GetCategoriesQuery(dbContext));

            Action act = () => sut.CreateCategory(new CreateCategoryCommand("Тест"));

            act.Should().NotThrow();
        }
    }
}
