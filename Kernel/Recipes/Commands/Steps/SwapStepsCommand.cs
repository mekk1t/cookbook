using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class SwapStepsCommand
    {
        public Guid FirstStepId { get; }
        public Guid SecondStepId { get; }

        public SwapStepsCommand(Guid firstStepId, Guid secondStepId)
        {
            FirstStepId = firstStepId;
            SecondStepId = secondStepId;
        }
    }
}
