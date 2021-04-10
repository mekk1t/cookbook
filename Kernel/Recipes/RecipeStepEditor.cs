using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Search;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeStepEditor
    {
        private readonly ICommand<EditStepPictureCommand> _editPicture;
        private readonly ICommand<EditStepDescriptionCommand> _editDescription;
        private readonly IQuery<RecipeStep, SearchStepQuery> _searchStep;
        private readonly ICommand<SwapStepsCommand> _swapSteps;

        public RecipeStepEditor(
            ICommand<EditStepPictureCommand> editPicture,
            ICommand<EditStepDescriptionCommand> editDescription,
            IQuery<RecipeStep, SearchStepQuery> searchStep,
            ICommand<SwapStepsCommand> swapSteps)
        {
            _editDescription = editDescription;
            _editPicture = editPicture;
            _searchStep = searchStep;
            _swapSteps = swapSteps;
        }

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
            var firstStep = _searchStep.Execute(new SearchStepQuery(firstStepId, new SearchStepQueryParameters(recipeId)));
            if (firstStep == null)
                throw new ArgumentException(null, nameof(firstStepId));
            var secondStep = _searchStep.Execute(new SearchStepQuery(secondStepId, new SearchStepQueryParameters(recipeId)));
            if (secondStep == null)
                throw new ArgumentException(null, nameof(secondStepId));

            _swapSteps.Execute(new SwapStepsCommand(firstStepId, secondStepId));
        }

        public void AppendStep(Guid recipeId, RecipeStep step)
        {

        }

        public void RemoveStep(Guid recipeId, Guid stepId)
        {

        }
    }
}
