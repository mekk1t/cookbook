using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; }
        public List<Category> Categories { get; }
        public List<Recipe> Recipes { get; }

        public Ingredient(Guid id, string name) : base(id)
        {
            Name = name;
            Categories = new List<Category>();
            Recipes = new List<Recipe>();
        }
    }
}
