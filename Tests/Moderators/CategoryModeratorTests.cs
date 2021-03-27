using DatabaseTestsConnector;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models.Commands;
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
            var sut = new CategoryModerator(new CreateCategoryCommandHandler(), new GetCategoriesQuery());

            sut.CreateCategory(new CreateCategoryCommand("Тест"));


        }

    }
}
