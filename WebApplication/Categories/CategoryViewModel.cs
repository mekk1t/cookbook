using KitProjects.MasterChef.WebApplication.Recipes;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoryViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<CategoryIngredientViewModel> Ingredients { get; }
        public IEnumerable<RecipeViewModel> Recipes { get; }

        public CategoryViewModel(Guid id, string name,
            IEnumerable<CategoryIngredientViewModel> ingredients,
            IEnumerable<RecipeViewModel> recipes)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
            Recipes = recipes;
        }
    }
}
