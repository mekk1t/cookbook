using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeStepEditor
    {
        private readonly ICommand<EditStepPictureCommand> _editPicture;
        private readonly ICommand<EditStepDescriptionCommand> _editDescription;
        private readonly IQuery<RecipeStep, SearchStepQuery> _searchStep;

        public void ChangePicture(Guid stepId, string newImage)
        {
            var step = _searchStep.Execute(new SearchStepQuery(stepId));
            if (step == null)
                throw new ArgumentException(null, nameof(stepId));

            // imageValidator.Validate(newImage);
            _editPicture.Execute(new EditStepPictureCommand(newImage, stepId));
        }

        public void ChangeDescription(Guid stepId, string newDescription)
        {
            var step = _searchStep.Execute(new SearchStepQuery(stepId));
            if (step == null)
                throw new ArgumentException(null, nameof(stepId));

            _editDescription.Execute(new EditStepDescriptionCommand(newDescription, stepId));
        }

        public void SwapSteps(Guid firstStepId, Guid secondStepId, Guid recipeId)
        {

        }

        public void AppendStep(Guid recipeId, RecipeStep step)
        {

        }

        public void RemoveStep(Guid recipeId, Guid stepId)
        {

        }
    }
}
