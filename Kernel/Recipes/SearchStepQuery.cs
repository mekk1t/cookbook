using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class SearchStepQuery
    {
        public Guid StepId { get; }

        public SearchStepQuery(Guid stepId)
        {
            StepId = stepId;
        }
    }
}
