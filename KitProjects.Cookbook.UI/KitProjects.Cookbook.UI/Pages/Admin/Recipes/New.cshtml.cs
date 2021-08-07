using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace KitProjects.Cookbook.UI.Pages.Admin.Recipes
{
    public class NewModel : PageModel
    {
        private readonly RecipeRepository _repository;

        public NewModel(RecipeRepository repository)
        {
            _repository = repository;
        }

        [BindRequired]
        public string Title { get; set; }
        [BindRequired]
        public string Description { get; set; }
        [BindRequired]
        public List<string> Tags { get; set; }
        [BindRequired]
        public List<Category> Categories { get; set; }
        [BindRequired]
        public List<IngredientDetails> Ingredients { get; set; }
        [BindRequired]
        public List<Step> Steps { get; set; }
        [BindRequired]
        public int CookingDuration { get; set; }
        [BindRequired]
        public List<CookingType> CookingTypes { get; set; }
        [BindRequired]
        public Difficulty Difficulty { get; set; }

        public void OnGet() { }
        public void OnPost()
        {
            _repository.Save(new Recipe
            {
                Title = Title,
                Description = Description,
                Tags = Tags,
                Categories = Categories,
                Difficulty = Difficulty,
                IngredientDetails = Ingredients,
                CookingTypes = CookingTypes,
                Steps = Steps,
                CookingDuration = CookingDuration
            });
        }
    }
}
