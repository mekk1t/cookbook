using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Recipe
    {
        public string Title { get; }
        public string Description { get; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
