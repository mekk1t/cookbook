using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Recipe : Entity
    {
        public Recipe(Guid id) : base(id)
        {
            Categories = new List<Category>();
            Ingredients = new List<Ingredient>();
            Steps = new List<RecipeStep>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<Category> Categories { get; }
        public List<Ingredient> Ingredients { get; }
        public List<RecipeStep> Steps { get; }
    }
}
