using System.Collections.Generic;

namespace KitProjects.Cookbook.Domain.Models
{
    public class Step : Entity
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public List<IngredientDetails> IngredientDetails { get; set; }
        public int? Duration { get; set; }
    }
}
