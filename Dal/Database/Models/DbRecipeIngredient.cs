using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;
using System.ComponentModel.DataAnnotations;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeIngredient
    {
        public Guid DbRecipeId { get; set; }
        public Guid DbIngredientId { get; set; }
        public DbRecipe DbRecipe { get; set; }
        public DbIngredient DbIngredient { get; set; }

        public Measures IngredientMeasure { get; set; }
        public decimal IngredientsAmount { get; set; }
        public string Notes { get; set; }
    }
}
