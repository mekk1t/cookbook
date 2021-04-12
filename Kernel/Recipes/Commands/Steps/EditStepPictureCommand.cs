using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class EditStepPictureCommand
    {
        public string NewImage { get; }
        public Guid StepId { get; }

        public EditStepPictureCommand(string newImage, Guid stepId)
        {
            NewImage = newImage;
            StepId = stepId;
        }
    }
}
