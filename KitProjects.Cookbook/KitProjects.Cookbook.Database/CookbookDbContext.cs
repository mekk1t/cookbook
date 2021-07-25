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
                category.Property(c => c.Name).IsRequired();
                category.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Ingredient>(ingredient =>
            {
                ingredient.HasIndex(i => i.Name).IsUnique();
                ingredient.Property(i => i.Name).IsRequired();
            });
        }
    }
}
