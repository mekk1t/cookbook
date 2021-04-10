using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class EditStepDescriptionCommand
    {
        public string NewDescription { get; }
        public Guid StepId { get; }

        public EditStepDescriptionCommand(string newDescription, Guid stepId)
        {
            NewDescription = newDescription;
            StepId = stepId;
        }
    }
}
