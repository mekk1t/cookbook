using System.Collections.Generic;
using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KitProjects.Cookbook.UI.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        public List<Recipe> Recipes { get; set; }

        private readonly RecipeRepository _repository;

        public IndexModel(RecipeRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Recipes = _repository.GetList();
        }
    }
}
