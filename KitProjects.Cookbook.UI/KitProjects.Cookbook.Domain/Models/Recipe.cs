using System;
using System.Collections.Generic;

namespace KitProjects.Cookbook.Domain.Models
{
    public class Recipe : Entity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public string ThumbnailBase64 { get; set; }
        public List<string> Tags { get; set; }
        public List<Category> Categories { get; set; }
        public List<IngredientDetails> IngredientDetails { get; set; }
        public List<Step> Steps { get; set; }
        public int CookingDuration { get; set; }
        public List<CookingType> CookingTypes { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
