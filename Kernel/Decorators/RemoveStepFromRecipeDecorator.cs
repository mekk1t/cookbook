using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class RemoveStepFromRecipeDecorator : ICommand<RemoveRecipeStepCommand>
    {
        private readonly ICommand<RemoveRecipeStepCommand> _decoratee;
        private readonly ICommand<NormalizeStepsOrderCommand> _normalizeStepsOrder;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;

        public RemoveStepFromRecipeDecorator(
            ICommand<RemoveRecipeStepCommand> decoratee,
            ICommand<NormalizeStepsOrderCommand> normalizeStepsOrder,
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe)
        {
            _decoratee = decoratee;
            _normalizeStepsOrder = normalizeStepsOrder;
            _getRecipe = getRecipe;
        }

        public void Execute(RemoveRecipeStepCommand command)
        {
            var recipe = _getRecipe.Execute(new GetRecipeQuery(command.RecipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var recipeStepsIds = recipe.Steps
                .OrderBy(step => step.Index)
                .Select(step => step.Id).ToList();
            var removingStepIndex = recipeStepsIds.IndexOf(command.StepId);
            if (removingStepIndex != recipe.Steps.Count - 1)
            {
                _decoratee.Execute(new RemoveRecipeStepCommand(command.RecipeId, command.StepId));
                _normalizeStepsOrder.Execute(new NormalizeStepsOrderCommand(command.RecipeId, removingStepIndex));
                return;
            }

            _decoratee.Execute(new RemoveRecipeStepCommand(command.RecipeId, command.StepId));
        }
    }
}
