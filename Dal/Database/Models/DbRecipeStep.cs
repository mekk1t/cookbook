using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipeStep
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ICollection<DbRecipeStepIngredient> StepIngredientsLink { get; set; }
    }
}
