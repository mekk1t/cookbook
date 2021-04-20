using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.EntityChecks;
using KitProjects.MasterChef.Kernel.Models.Queries.Search;
using KitProjects.MasterChef.Kernel.Recipes;
using System;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class RecipeStepChecker : IEntityChecker<RecipeStep, StepEntityCheckParameters>
    {
        private readonly IQuery<RecipeStep, SearchStepQuery> _searchStep;

        public RecipeStepChecker(IQuery<RecipeStep, SearchStepQuery> searchStep)
        {
            _searchStep = searchStep;
        }

        public bool CheckExistence(StepEntityCheckParameters parameters)
        {
            if (parameters.RecipeId == Guid.Empty || parameters.StepId == Guid.Empty)
                throw new ArgumentException(null, nameof(parameters));

            var step = _searchStep.Execute(new SearchStepQuery(parameters.StepId, new SearchStepQueryParameters(parameters.RecipeId)));
            if (step == null)
                return false;

            return true;
        }
    }
}
