using KitProjects.Cookbook.Database;
using System;

namespace KitProjects.Cookbook.Tests.Database
{
    public abstract class DatabaseTests : IDisposable
    {
        protected readonly CookbookDbContext _dbContext;

        public DatabaseTests(DbFixture fixture)
        {
            _dbContext = fixture.DbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
