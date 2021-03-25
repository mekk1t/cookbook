using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Ingredient
    {
        public string Name { get; }
        public ICollection<Category> Categories { get; }
    }
}
