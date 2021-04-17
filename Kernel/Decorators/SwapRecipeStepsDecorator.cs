using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.EntityChecks;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class SwapRecipeStepsDecorator : ICommand<SwapStepsCommand>
    {
        private readonly ICommand<SwapStepsCommand> _decoratee;
        private readonly IEntityChecker<RecipeStep, StepEntityCheckParameters> _stepChecker;

        public SwapRecipeStepsDecorator(
            ICommand<SwapStepsCommand> decoratee,
            IEntityChecker<RecipeStep, StepEntityCheckParameters> stepChecker)
        {
            _decoratee = decoratee;
            _stepChecker = stepChecker;
        }

        public void Execute(SwapStepsCommand command)
        {
            bool firstStepExists = _stepChecker.CheckExistence(new StepEntityCheckParameters(command.RecipeId, command.FirstStepId));
            if (!firstStepExists)
                throw new ArgumentException(null, nameof(command));

            bool secondStepExists = _stepChecker.CheckExistence(new StepEntityCheckParameters(command.RecipeId, command.SecondStepId));
            if (!secondStepExists)
                throw new ArgumentException(null, nameof(command));

            _decoratee.Execute(command);
        }
    }
}
