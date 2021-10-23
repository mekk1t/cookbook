using KP.Cookbook.Database;
using KP.Cookbook.Domain.Models;
using KP.Cookbook.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KP.Cookbook.UI.Pages.Recipes
{
    public class EditModel : PageModel
    {
        private readonly Repository<Ingredient> _ingredientRepository;
        private readonly RecipeRepository _repository;
        [BindProperty] public Recipe Recipe { get; set; }
        [BindProperty] public IFormFile NewThumbnail { get; set; }
        [BindProperty] public List<StepFormModel> StepForms { get; set; } = new List<StepFormModel>();

        public EditModel(RecipeRepository repository, Repository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
            _repository = repository;
        }

        public void OnGet(long id)
        {
            Recipe = _repository.GetDetails(id);
        }

        public void OnPost()
        {
            foreach (var ingredientDetails in Recipe.IngredientDetails)
            {
                foreach (var step in Recipe.Steps)
                {
                    if (step.IngredientDetails != null)
                    {
                        var stepIngredientDetails = step.IngredientDetails.FirstOrDefault(details => details.Ingredient.Id == ingredientDetails.Ingredient.Id);
                        if (stepIngredientDetails != null)
                        {
                            stepIngredientDetails.Ingredient = ingredientDetails.Ingredient;
                        }
                    }
                }
            }

            foreach (var step in StepForms)
            {
                if (step.Picture != null)
                {
                    using var stream = new MemoryStream();
                    step.Picture.CopyTo(stream);
                    Recipe.Steps.First(recipeStep => recipeStep.Order == step.StepOrder).ImageBase64 = Convert.ToBase64String(stream.ToArray());
                }
            }

            if (NewThumbnail != null)
            {
                using var ms = new MemoryStream();
                NewThumbnail.CopyTo(ms);
                Recipe.ThumbnailBase64 = Convert.ToBase64String(ms.ToArray());
            }

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