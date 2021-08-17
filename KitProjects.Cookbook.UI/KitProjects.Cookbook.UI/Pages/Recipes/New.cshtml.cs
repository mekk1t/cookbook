using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using KitProjects.Cookbook.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace KitProjects.Cookbook.UI.Pages.Recipes
{
    public class NewModel : PageModel
    {
        private readonly Repository<Ingredient> _ingredientRepository;
        private readonly RecipeRepository _repository;
        [BindProperty] public Recipe Recipe { get; set; }

        public NewModel(RecipeRepository repository, Repository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
            _repository = repository;
        }

        public void OnPost()
        {
            _repository.Save(Recipe);
        }

        public JsonResult OnPostNewIngredient([FromBody] string data)
        {
            var ingredient = JsonSerializer.Deserialize<Ingredient>(data);
            _ingredientRepository.Save(ingredient);
            return new JsonResult(new { ingredient.Id });
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
