using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class IngredientChecker : IEntityChecker<Ingredient, string>
    {
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;

        public IngredientChecker(IQuery<Ingredient, GetIngredientQuery> getIngredient)
        {
            _getIngredient = getIngredient;
        }

        public bool CheckExistence(string parameters = null)
        {
            if (parameters.IsNullOrEmpty())
                return false;

            var ingredient = _getIngredient.Execute(new GetIngredientQuery(parameters));
            if (ingredient == null)
                return false;

            return true;
        }
    }
}
