using Xunit;

namespace KitProjects.Cookbook.Tests.DatabaseTests
{
    [CollectionDefinition(nameof(DbFixture))]
    public class DbFixtureCollectionDefinition : IClassFixture<DbFixture>
    {
    }
}
