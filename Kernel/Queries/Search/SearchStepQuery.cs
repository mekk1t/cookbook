using KitProjects.MasterChef.Kernel.Models.Queries.Search;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class SearchStepQuery
    {
        public Guid StepId { get; }
        public SearchStepQueryParameters Parameters { get; }

        public SearchStepQuery(Guid stepId, SearchStepQueryParameters parameters = null)
        {
            StepId = stepId;
            Parameters = parameters;
        }
    }
}
