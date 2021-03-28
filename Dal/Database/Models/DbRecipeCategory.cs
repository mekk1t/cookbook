 using System;
using System.ComponentModel.DataAnnotations;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeCategory
    {
        public Guid DbRecipeId { get; set; }
        public Guid DbCategoryId { get; set; }
        public DbRecipe DbRecipe { get; set; }
        public DbCategory DbCategory { get; set; }
    }
}
