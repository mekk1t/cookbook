using System.Collections.Generic;

namespace KitProjects.Cookbook.Core.Models
{
    public class Ingredient : Entity
    {
        public string Name { get; set; }
        public List<Category> Categories { get; } = new List<Category>();

        public Ingredient()
        {

        }

        public Ingredient(long id) : base(id)
        {

        }

        public Ingredient(Ingredient other) : base(other.Id)
        {
            Name = other.Name;
        }
    }
}
