using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Domain.Models;
using KitProjects.Cookbook.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void OnPostNewIngredient([FromBody] Ingredient ingredient)
        {
            _ingredientRepository.Save(ingredient);
        }

        public PartialViewResult OnGetIngredientToRecipe([FromQuery] int order) =>
            Partial("_RecipeIngredient", new IngredientFormModel
            {
                Prefix = "Recipe.IngredientDetails",
                Order = order
            });

        public PartialViewResult OnGetIngredientToStep([FromQuery] int stepOrder, [FromQuery] int ingredientOrder) =>
            Partial("_RecipeIngredient", new IngredientFormModel
            {
                Prefix = $"Recipe.Steps[{stepOrder}].IngredientDetails",
                Order = ingredientOrder
            });

        public PartialViewResult OnGetStepToRecipe([FromQuery] int order) => Partial("_RecipeStep", order);
    }
}
