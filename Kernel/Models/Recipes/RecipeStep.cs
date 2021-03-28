using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeStep
    {
        public int Index { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
