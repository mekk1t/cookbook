using KitProjects.MasterChef.Dal.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KitProjects.MasterChef.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<DbCategory> Categories { get; set; }
        public DbSet<DbIngredient> Ingredients { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            this.ChangeTracker.Clear();
            return result;
        }
    }
}
