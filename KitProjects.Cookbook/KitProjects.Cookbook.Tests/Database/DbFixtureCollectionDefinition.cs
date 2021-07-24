using Xunit;

namespace KitProjects.Cookbook.Tests.Database
{
    [CollectionDefinition(nameof(DbFixture))]
    public class DbFixtureCollectionDefinition : IClassFixture<DbFixture>
    {
    }
}
