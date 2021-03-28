using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; }
        public ICollection<Category> Categories { get; }

        public Ingredient(Guid id, string name, ICollection<Category> categories) : base(id)
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
