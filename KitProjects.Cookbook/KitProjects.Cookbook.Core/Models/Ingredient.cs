using System.Collections.Generic;

namespace KitProjects.Cookbook.Core.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; set; }
        public List<Category> Categories { get; set; }

        public Ingredient()
        {

        }

        public Ingredient(long id) : base(id)
        {

        }
    }
}
