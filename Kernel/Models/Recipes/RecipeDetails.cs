using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeDetails
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public List<Category> Categories { get; } = new List<Category>();
        public List<RecipeStep> Steps { get; } = new List<RecipeStep>();
        public List<RecipeIngredientDetails> Ingredients { get; } = new List<RecipeIngredientDetails>();
    }
}
