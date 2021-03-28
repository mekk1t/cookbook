using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoryViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<CategoryIngredientViewModel> Ingredients { get; }

        public CategoryViewModel(Guid id, string name, IEnumerable<CategoryIngredientViewModel> ingredients)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
        }
    }
}
