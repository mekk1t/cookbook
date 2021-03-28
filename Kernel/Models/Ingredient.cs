using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; }
        public IEnumerable<Category> Categories { get; }

        public Ingredient(Guid id, string name, IEnumerable<Category> categories) : base(id)
        {
            Name = name;
            Categories = categories;
        }

        public Ingredient(Guid id, string name) : base(id)
        {
            Name = name;
            Categories = new List<Category>();
        }

        public Ingredient()
        {
        }
    }
}
