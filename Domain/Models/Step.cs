using System.Collections.Generic;

namespace KP.Cookbook.Domain.Models
{
    public class Step : Entity
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public List<IngredientDetails> IngredientDetails { get; set; } = new List<IngredientDetails>(0);
        public int? Duration { get; set; }
    }
}
