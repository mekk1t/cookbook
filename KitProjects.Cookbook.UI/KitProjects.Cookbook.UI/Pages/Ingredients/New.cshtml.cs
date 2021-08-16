using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KitProjects.Cookbook.UI.Pages.Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly Repository<Ingredient> _repository;

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public IngredientType Type { get; set; }

        public IndexModel(Repository<Ingredient> repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            _repository.Save(new Ingredient { Name = Name, Type = Type });
        }
    }
}
