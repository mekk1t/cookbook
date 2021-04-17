using System;

namespace KitProjects.MasterChef.Kernel.Recipes.Commands
{
    public class EditStepDescriptionCommand
    {
        public string NewDescription { get; }
        public Guid StepId { get; }
        public Guid RecipeId { get; }

        public EditStepDescriptionCommand(string newDescription, Guid stepId, Guid recipeId)
        {
            RecipeId = recipeId;
            NewDescription = newDescription;
            StepId = stepId;
        }
    }
}
