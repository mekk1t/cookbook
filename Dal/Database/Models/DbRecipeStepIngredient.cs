using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;
using System.ComponentModel.DataAnnotations;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeStepIngredient
    {
        public Guid DbRecipeStepId { get; set; }
        public Guid DbIngredientId { get; set; }
        public DbRecipeStep DbRecipeStep { get; set; }
        public DbIngredient DbIngredient { get; set; }

        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
    }
}
