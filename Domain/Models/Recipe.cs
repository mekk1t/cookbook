using System;
using System.Collections.Generic;

namespace KP.Cookbook.Domain.Models
{
    public class Recipe : Entity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public string ThumbnailBase64 { get; set; }
        public List<string> Tags { get; set; } = new List<string>(0);
        public List<IngredientDetails> IngredientDetails { get; set; } = new List<IngredientDetails>(0);
        public List<Step> Steps { get; set; } = new List<Step>(0);
        public int CookingDuration { get; set; }
        public Source Source { get; set; }
    }
}
