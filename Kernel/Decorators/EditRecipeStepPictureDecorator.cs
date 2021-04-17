using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.EntityChecks;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class EditRecipeStepPictureDecorator : ICommand<EditStepPictureCommand>
    {
        private readonly ICommand<EditStepPictureCommand> _decoratee;
        private readonly IEntityChecker<RecipeStep, StepEntityCheckParameters> _stepChecker;

        public EditRecipeStepPictureDecorator(
            ICommand<EditStepPictureCommand> decoratee,
            IEntityChecker<RecipeStep, StepEntityCheckParameters> stepChecker)
        {
            _decoratee = decoratee;
            _stepChecker = stepChecker;
        }

        public void Execute(EditStepPictureCommand command)
        {
            bool stepExists = _stepChecker.CheckExistence(new StepEntityCheckParameters(command.RecipeId, command.StepId));
            if (!stepExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
