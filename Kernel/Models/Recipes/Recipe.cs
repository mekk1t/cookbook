using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<RecipeStep> Steps { get; set; }
    }
}
