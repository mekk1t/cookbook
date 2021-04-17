using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; }
        public List<Category> Categories { get; } = new List<Category>();
        public List<Recipe> Recipes { get; } = new List<Recipe>();

        public Ingredient(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public Ingredient(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}
