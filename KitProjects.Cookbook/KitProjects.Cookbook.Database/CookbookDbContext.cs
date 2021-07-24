using KitProjects.Cookbook.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KitProjects.Cookbook.Database
{
    public class CookbookDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public CookbookDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(category =>
            {
                category.Property(c => c.Type).HasConversion<int>();
                category.Property(c => c.Name).IsRequired();
            });
        }
    }
}
