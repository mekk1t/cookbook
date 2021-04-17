using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.EntityChecks;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class EditRecipeStepDescriptionDecorator : ICommand<EditStepDescriptionCommand>
    {
        private readonly ICommand<EditStepDescriptionCommand> _decoratee;
        private readonly IEntityChecker<RecipeStep, StepEntityCheckParameters> _stepChecker;

        public EditRecipeStepDescriptionDecorator(
            ICommand<EditStepDescriptionCommand> decoratee,
            IEntityChecker<RecipeStep, StepEntityCheckParameters> stepChecker)
        {
            _decoratee = decoratee;
            _stepChecker = stepChecker;
        }

        public void Execute(EditStepDescriptionCommand command)
        {
            bool stepExists = _stepChecker.CheckExistence(new StepEntityCheckParameters(command.RecipeId, command.StepId));
            if (!stepExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
