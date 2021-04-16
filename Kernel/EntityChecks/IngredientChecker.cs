using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using System;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class IngredientChecker : IEntityChecker<Ingredient, string>, IEntityChecker<Ingredient, Guid>
    {
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;

        public IngredientChecker(IQuery<Ingredient, GetIngredientQuery> getIngredient)
        {
            _getIngredient = getIngredient;
        }

        public bool CheckExistence(string parameters)
        {
            if (parameters.IsNullOrEmpty())
                return false;

            var ingredient = _getIngredient.Execute(new GetIngredientQuery(parameters));
            if (ingredient == null)
                return false;

            return true;
        }

        public bool CheckExistence(Guid parameters)
        {
            if (parameters == Guid.Empty)
                throw new ArgumentException(null, nameof(parameters));

            var ingredient = _getIngredient.Execute(new GetIngredientQuery(parameters));
            if (ingredient == null)
                return false;

            return true;
        }
    }
}
