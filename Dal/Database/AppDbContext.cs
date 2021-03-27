﻿using KitProjects.MasterChef.Dal.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace KitProjects.MasterChef.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<DbRecipe> Recipes { get; set; }
        public DbSet<DbIngredient> Ingredients { get; set; }
        public DbSet<DbCategory> Categories { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
