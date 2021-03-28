using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeStepIngredient
    {
        public Guid DbRecipeStepId { get; set; }
        public Guid DbIngredientId { get; set; }
        public DbRecipeStep Step { get; set; }
        public DbIngredient Ingredient { get; set; }

        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
    }
}
