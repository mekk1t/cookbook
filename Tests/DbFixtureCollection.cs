using KitProjects.Fixtures;
using Xunit;

namespace DatabaseTestsConnector
{
    [CollectionDefinition("Db")]
    public class DbFixtureCollection : ICollectionFixture<DbFixture>
    {
    }
}
