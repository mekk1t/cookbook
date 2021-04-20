using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class EditStepPictureCommand
    {
        public string NewImage { get; }
        public Guid StepId { get; }
        public Guid RecipeId { get; }

        public EditStepPictureCommand(string newImage, Guid stepId, Guid recipeId)
        {
            RecipeId = recipeId;
            NewImage = newImage;
            StepId = stepId;
        }
    }
}
