using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KP.Cookbook.Database
{
    public class DbContextFactory : IDesignTimeDbContextFactory<CookbookDbContext>
    {
        public CookbookDbContext CreateDbContext(string[] args)
        {
            var connectionString = args[0];
            var builder = new DbContextOptionsBuilder<CookbookDbContext>();
            return new CookbookDbContext(builder.UseSqlServer(connectionString).Options);
        }
    }
}
