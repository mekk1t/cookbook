using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeIngredientEditor
    {
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;
        private readonly ICommand<AppendRecipeIngredientCommand> _appendIngredient;
        private readonly IngredientService _ingredientService;

        public void AppendIngredient(Guid recipeId, Ingredient ingredient)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            var existingIngredient = _searchIngredient.Execute(new SearchIngredientQuery(ingredient.Id));
            if (existingIngredient == null)
                _ingredientService.CreateIngredient(new CreateIngredientCommand(ingredient.Name, Array.Empty<string>()));

            _appendIngredient.Execute(new AppendRecipeIngredientCommand(recipeId, ingredient.Id));
        }

        public void RemoveIngredient(Guid recipeId, Guid ingredientId)
        {

        }

        public void ReplaceIngredient(Ingredient oldIngredient, Ingredient newIngredient, Guid recipeId)
        {
            if (oldIngredient == newIngredient)
                return;

            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));


        }

        public void ReplaceIngredientsList(Ingredient[] oldIngredients, Ingredient[] newIngredients)
        {

        }
    }
}
