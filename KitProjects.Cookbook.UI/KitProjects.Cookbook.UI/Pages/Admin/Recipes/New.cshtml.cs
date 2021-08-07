using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KitProjects.Cookbook.UI.Pages.Admin.Recipes
{
    public class NewModel : PageModel
    {
        private readonly RecipeRepository _repository;

        public NewModel(RecipeRepository repository)
        {
            _repository = repository;
        }

        public void OnGet() { }
        public void OnPost(Recipe recipe)
        {
            _repository.Save(recipe);
        }
    }
}
