using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class RecipeService
    {
        private readonly ICommand<CreateRecipeCommand> _createRecipeCommand;
        private readonly CategoryService _categoryService;
        private readonly IngredientService _ingredientService;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;

        public RecipeService(
            ICommand<CreateRecipeCommand> createRecipeCommand,
            CategoryService categoryService,
            IngredientService ingredientService,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient)
        {
            _createRecipeCommand = createRecipeCommand;
            _categoryService = categoryService;
            _ingredientService = ingredientService;
            _searchIngredient = searchIngredient;
        }

        public void CreateRecipe(CreateRecipeCommand command)
        {
            if (command.IngredientsDetails?.Count() > 0)
            {
                foreach (var ingredient in command.IngredientsDetails)
                {
                    var oldIngredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredient.IngredientName));
                    if (oldIngredient == null)
                    {
                        _ingredientService.CreateIngredient(new CreateIngredientCommand(ingredient.IngredientName, new List<string>()));
                    }
                }
            }

            if (command.Categories?.Count() > 0)
            {
                foreach (var category in command.Categories)
                {
                    var oldCategory = _categoryService.GetCategories(new GetCategoriesQuery(limit: 1000))
                        .FirstOrDefault(c => c.Name == category);
                    if (oldCategory == null)
                    {
                        _categoryService.CreateCategory(new CreateCategoryCommand(category));
                    }
                }
            }

            _createRecipeCommand.Execute(command);
        }
    }
}
