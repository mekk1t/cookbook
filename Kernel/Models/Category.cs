using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Category : Entity
    {
        public Category()
        {
        }

        public Category(Guid id) : base(id)
        {
        }

        public Category(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public Category(Guid id, string name, IEnumerable<Ingredient> ingredients) : base(id)
        {
            Name = name;
            Ingredients = ingredients;
        }

        public string Name { get; }
        public IEnumerable<Ingredient> Ingredients { get; }
    }
}
