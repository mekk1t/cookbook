using KitProjects.Cookbook.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace KitProjects.Cookbook.Tests.Database
{
    public sealed class DbFixture : IDisposable
    {
        public CookbookDbContext DbContext => new(new DbContextOptionsBuilder<CookbookDbContext>().UseSqlServer(Connection).Options);

        private static readonly object _lock = new object();
        private static bool _databaseInitialized;
        public DbConnection Connection { get; }

        public DbFixture()
        {
            Connection = new SqlConnection("Server=localhost;Database=MasterChefTests;Trusted_Connection=True;");
            Seed();
            Connection.Open();
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var dbContext = DbContext)
                    {
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        dbContext.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}
