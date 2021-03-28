using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeIngredient
    {
        public Guid DbRecipeId { get; set; }
        public Guid DbIngredientId { get; set; }
        public DbRecipe Recipe { get; set; }
        public DbIngredient Ingredient { get; set; }

        public Measures IngredientMeasure { get; set; }
        public decimal IngredientxAmount { get; set; }
        public string Notes { get; set; }
    }
}
