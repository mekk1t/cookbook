using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace KitProjects.Fixtures
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
            dbContext.Categories.Add(new DbCategory(category.Id, category.Name));
            dbContext.SaveChanges();
        }

        public Category FindCategory(string name)
        {
            using var dbContext = this.DbContext;
            var dbCategory = dbContext.Categories.AsNoTracking().FirstOrDefault(r => r.Name == name);
            return new Category(dbCategory.Id, dbCategory.Name);
        }

        public void SeedIngredient(Ingredient ingredient)
        {
            using var dbContext = this.DbContext;
            dbContext.Ingredients.Add(new DbIngredient(
                ingredient.Id, ingredient.Name,
                ingredient.Categories.Select(c => new DbCategory(c.Id, c.Name)).ToList()));
            dbContext.SaveChanges();
        }

        public Ingredient FindIngredient(string name)
        {
            using var dbContext = this.DbContext;
            var dbIngredient = dbContext.Ingredients
                .AsNoTracking()
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Name == name);

            if (dbIngredient == null)
            {
                return null;
            }
            else
            {
                var ingredient = new Ingredient(dbIngredient.Id, dbIngredient.Name);
                ingredient.Categories.AddRange(dbIngredient.Categories.Select(c => new Category(c.Id, c.Name)));
                return ingredient;
            }
        }

        public void SeedRecipe(DbRecipe recipe)
        {
            using var dbContext = this.DbContext;
            dbContext.Recipes.Add(recipe);
            dbContext.SaveChanges();
        }

        public DbRecipe FindRecipe(Guid recipeId)
        {
            using var dbContext = this.DbContext;
            return dbContext.Recipes.AsNoTracking()
                .Include(r => r.RecipeCategoriesLink).ThenInclude(link => link.DbCategory)
                .Include(r => r.RecipeIngredientLink).ThenInclude(link => link.DbIngredient)
                .Include(r => r.Steps)
                .First(r => r.Id == recipeId);
        }

        public void Dispose() => Connection.Dispose();
    }
}
