using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.EntityChecks;
using KitProjects.MasterChef.Kernel.Queries.Get;
using System;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class RecipeStepChecker : IEntityChecker<RecipeStep, StepEntityCheckParameters>
    {
        private readonly IQuery<RecipeStep, GetRecipeStepQuery> _getStep;

        public RecipeStepChecker(IQuery<RecipeStep, GetRecipeStepQuery> getStep)
        {
            _getStep = getStep;
        }

        public bool CheckExistence(StepEntityCheckParameters parameters)
        {
            if (parameters.RecipeId == Guid.Empty || parameters.StepId == Guid.Empty)
                throw new ArgumentException(null, nameof(parameters));

            var step = _getStep.Execute(new GetRecipeStepQuery(parameters.RecipeId, parameters.StepId));
            if (step == null)
                return false;

            return true;
        }
    }
}
