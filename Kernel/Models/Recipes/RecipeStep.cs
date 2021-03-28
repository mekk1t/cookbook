using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeStep : Entity
    {
        public RecipeStep()
        {
        }

        public RecipeStep(Guid id) : base(id)
        {
        }

        public int Index { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}
