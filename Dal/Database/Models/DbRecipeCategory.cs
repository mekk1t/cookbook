using System;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeCategory
    {
        public Guid DbRecipeId { get; set; }
        public Guid DbCategoryId { get; set; }
        public DbRecipe Recipe { get; set; }
        public DbCategory Category { get; set; }
    }
}
