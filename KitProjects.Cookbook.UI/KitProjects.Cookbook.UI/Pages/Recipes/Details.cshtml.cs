using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KitProjects.Cookbook.UI.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        public Recipe Recipe { get; set; }
        private readonly RecipeRepository _repository;

        public DetailsModel(RecipeRepository repository)
        {
            _repository = repository;
        }

        public void OnGet(long id)
        {
            Recipe = _repository.GetDetails(id);
        }
    }
}
