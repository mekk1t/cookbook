using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; }
        public ICollection<Category> Categories { get; }

        public Ingredient(string name, ICollection<Category> categories)
        {
            Name = name;
            Categories = categories;
        }
    }
}
