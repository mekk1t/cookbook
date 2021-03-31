using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.WebApplication.Recipes;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class GetCategoriesResponse
    {
        public IEnumerable<CategoryViewModel> Categories { get; }

        public GetCategoriesResponse(IEnumerable<Category> categories)
        {
            this.Categories = categories.Select(c => new CategoryViewModel(
                c.Id, c.Name,
                c.Ingredients.Select(i => new CategoryIngredientViewModel(i.Id, i.Name)),
                c.Recipes.Select(r => new RecipeViewModel(r.Id, r.Title, r.Description))));
        }
    }
}
