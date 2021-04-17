using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using System;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class RecipeChecker : IEntityChecker<Recipe, Guid>
    {
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;

        public RecipeChecker(IQuery<RecipeDetails, GetRecipeQuery> getRecipe)
        {
            _getRecipe = getRecipe;
        }

        public bool CheckExistence(Guid parameters)
        {
            if (parameters == Guid.Empty)
                throw new ArgumentException(null, nameof(parameters));

            var recipe = _getRecipe.Execute(new GetRecipeQuery(parameters));
            if (recipe == null)
                return false;

            return true;
        }
    }
}
