using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<DbCategory> Categories { get; set; }
        public IEnumerable<DbRecipeIngredient> RecipeIngredientLink { get; set; }
        public ICollection<DbRecipeStep> Steps { get; set; }
    }
}
