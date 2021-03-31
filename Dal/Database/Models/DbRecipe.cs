using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbRecipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<DbRecipeCategory> RecipeCategoriesLink { get; set; }
        public ICollection<DbRecipeIngredient> RecipeIngredientLink { get; set; }
        public ICollection<DbRecipeStep> Steps { get; set; }
    }
}
