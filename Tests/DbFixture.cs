using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace DatabaseTestsConnector
{
    public sealed class DbFixture : IDisposable
    {
        public AppDbContext DbContext => new(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Connection).Options);

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
                    using (var dbContext = this.DbContext)
                    {
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        dbContext.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void SeedCategory(Category category)
        {
            using var dbContext = this.DbContext;
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
        }

        public void Dispose() => Connection.Dispose();
    }
}
