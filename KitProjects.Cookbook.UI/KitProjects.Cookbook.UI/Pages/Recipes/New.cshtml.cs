using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using KitProjects.Cookbook.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KitProjects.Cookbook.UI.Pages.Recipes
{
    public class NewModel : PageModel
    {
        private readonly Repository<Ingredient> _ingredientRepository;
        private readonly RecipeRepository _repository;
        [BindProperty] public Recipe Recipe { get; set; }
        [BindProperty] public IFormFile Thumbnail { get; set; }
        [BindProperty] public List<StepFormModel> StepForms { get; set; }

        public NewModel(RecipeRepository repository, Repository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
            _repository = repository;
        }

        public void OnPost()
        {
            foreach (var ingredientDetails in Recipe.IngredientDetails)
            {
                foreach (var step in Recipe.Steps)
                {
                    var stepIngredientDetails = step.IngredientDetails.FirstOrDefault(details => details.Ingredient.Id == ingredientDetails.Ingredient.Id);
                    if (stepIngredientDetails != null)
                    {
                        stepIngredientDetails.Ingredient = ingredientDetails.Ingredient;
                    }
                }
            }

            using var ms = new MemoryStream();
            Thumbnail.CopyTo(ms);
            Recipe.ThumbnailBase64 = Convert.ToBase64String(ms.ToArray());

            _repository.Save(Recipe);
        }

        public PartialViewResult OnGetIngredientToRecipe([FromQuery] int order, [FromQuery] long ingredientId)
        {
            return Partial("_IngredientForm", new IngredientFormModel
            {
                Prefix = "Recipe.IngredientDetails",
                Order = order,
                Ingredient = _ingredientRepository.GetOrDefault(ingredientId)
            });
        }


        public PartialViewResult OnGetIngredientToStep([FromQuery] int stepOrder, [FromQuery] int ingredientOrder, [FromQuery] long ingredientId) =>
            Partial("_IngredientForm", new IngredientFormModel
            {
                Prefix = $"Recipe.Steps[{stepOrder}].IngredientDetails",
                Order = ingredientOrder,
                Ingredient = _ingredientRepository.GetOrDefault(ingredientId)
            });

        public PartialViewResult OnGetStepToRecipe([FromQuery] int order) => Partial("_RecipeStep", order);
    }
}
