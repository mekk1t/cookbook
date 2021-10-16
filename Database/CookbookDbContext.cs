using KitProjects.Cookbook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.Cookbook.Database
{
    public class CookbookDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Source> Sources { get; set; }

        public CookbookDbContext(DbContextOptions<CookbookDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>(builder =>
            {
                builder.OwnsMany(recipe => recipe.IngredientDetails)
                    .Property(details => details.Amount).HasPrecision(8, 2);
                builder
                    .Property(r => r.Tags)
                    .HasConversion(
                    convertToProviderExpression: tagsList => string.Join(';', tagsList),
                    convertFromProviderExpression: tagsString => tagsString.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            modelBuilder.Entity<Step>(builder =>
            {
                builder.OwnsMany(step => step.IngredientDetails)
                    .Property(details => details.Amount).HasPrecision(8, 2);
            });
        }
    }
}
