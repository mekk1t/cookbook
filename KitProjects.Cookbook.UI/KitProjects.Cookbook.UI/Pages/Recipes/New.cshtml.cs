using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace KitProjects.Cookbook.UI.Pages.Recipes
{
    public class NewModel : PageModel
    {
        private readonly RecipeRepository _repository;
        [BindProperty] public Recipe Recipe { get; set; }

        public NewModel(RecipeRepository repository)
        {
            _repository = repository;
        }

        public void OnPost()
        {
            _repository.Save(Recipe);
        }
    }
}
