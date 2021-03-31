using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Category : Entity
    {
        public string Name { get; }
        public List<Ingredient> Ingredients { get; }
        public List<Recipe> Recipes { get; }

        public Category(Guid id, string name) : base(id)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Recipes = new List<Recipe>();
        }
    }
}
