using KitProjects.MasterChef.WebApplication.Recipes;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoryViewModel
    {
        /// <summary>
        /// ID категории в формате GUID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Список ингредиентов этой категории.
        /// </summary>
        public IEnumerable<CategoryIngredientViewModel> Ingredients { get; }
        /// <summary>
        /// Список рецептов этой категории.
        /// </summary>
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
