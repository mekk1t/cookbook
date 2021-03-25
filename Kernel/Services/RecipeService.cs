using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Services
{
    public class RecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;

        public RecipeService(IRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public IEnumerable<Recipe> SearchRecipes(RecipeFilter filter) =>
            _recipeRepository.Read(c =>
                c.Title.ContainsIgnoreCase(filter.SearchTerm) ||
                c.Description.ContainsIgnoreCase(filter.SearchTerm) ||
                c.Categories.Any(category => category.Name.ContainsIgnoreCase(filter.SearchTerm)) ||
                c.Categories.Any(category => category.Name.EqualsIgnoreCase(filter.CategoryName)) ||
                c.Steps.Count >= filter.StepsCount ||
                c.Ingredients
                    .Any(ingredient =>
                        ingredient.Categories
                            .Any(category => category.Name.ContainsIgnoreCase(filter.IngredientCategory))));
    }
}
