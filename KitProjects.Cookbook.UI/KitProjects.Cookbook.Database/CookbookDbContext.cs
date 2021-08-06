using KitProjects.Cookbook.Database.Extensions;
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

        public CookbookDbContext(DbContextOptions<CookbookDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>(builder =>
            {
                builder.OwnsMany(recipe => recipe.IngredientDetails)
                    .Property(details => details.Amount).HasPrecision(4, 2);
                builder
                    .Property(r => r.Tags)
                    .HasConversion(
                    convertToProviderExpression: tagsList => string.Join(';', tagsList),
                    convertFromProviderExpression: tagsString => tagsString.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
                builder
                    .Property(r => r.Categories)
                    .HasConversion(
                    categories => string.Join(';', categories.Select(c => c.ToString())),
                    categoriesString => categoriesString
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.ToEnum<Category>())
                        .ToList());
                builder
                    .Property(r => r.CookingTypes)
                    .HasConversion(
                    types => string.Join(';', types.Select(c => c.ToString())),
                    typesString => typesString
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.ToEnum<CookingType>())
                        .ToList());
            });

            modelBuilder.Entity<Step>(builder =>
            {
                builder.OwnsMany(step => step.IngredientDetails)
                    .Property(details => details.Amount).HasPrecision(4, 2);
            });
        }
    }
}
