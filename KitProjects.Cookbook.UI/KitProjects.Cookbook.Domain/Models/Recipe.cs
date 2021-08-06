using System.Collections.Generic;

namespace KitProjects.Cookbook.Domain.Models
{
    public class Recipe : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Category> Categories { get; set; }
    }
}
