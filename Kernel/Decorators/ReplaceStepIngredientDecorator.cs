using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Models.Steps;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class ReplaceStepIngredientDecorator : ICommand<ReplaceStepIngredientCommand>
    {
        private readonly ICommand<ReplaceStepIngredientCommand> _decoratee;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;
        private readonly ICommand<AppendRecipeIngredientCommand> _appendIngredientToRecipe;
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;

        public ReplaceStepIngredientDecorator(
            ICommand<ReplaceStepIngredientCommand> decoratee,
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe,
            ICommand<AppendRecipeIngredientCommand> appendIngredientToRecipe,
            IQuery<Ingredient, GetIngredientQuery> getIngredient)
        {
            _decoratee = decoratee;
            _getIngredient = getIngredient;
            _getRecipe = getRecipe;
            _appendIngredientToRecipe = appendIngredientToRecipe;
        }

        public void Execute(ReplaceStepIngredientCommand command)
        {
            if (command.OldIngredient == command.NewIngredient)
                throw new InvalidOperationException();

            var recipe = _getRecipe.Execute(new GetRecipeQuery(command.Ids.Recipe));
            if (recipe == null)
                throw new EntityNotFoundException();

            var step = recipe.Steps.FirstOrDefault(step => step.Id == command.Ids.Step);
            if (step == null)
                throw new EntityNotFoundException();

            var oldIngredientFromStep = step.IngredientsDetails.FirstOrDefault(details => details.IngredientName == command.OldIngredient.Name);
            if (oldIngredientFromStep == null)
                throw new InvalidOperationException();

            if (!recipe.Ingredients.Select(i => i.IngredientName).Contains(command.NewIngredient.Name))
            {
                _appendIngredientToRecipe.Execute(
                    new AppendRecipeIngredientCommand(
                        command.Ids.Recipe,
                        command.NewIngredient,
                        new AppendIngredientParameters(
                            oldIngredientFromStep.Amount,
                            oldIngredientFromStep.Measure)));
                var appendedIngredient = _getIngredient.Execute(new GetIngredientQuery(command.NewIngredient.Name));
                command =
                    new ReplaceStepIngredientCommand(
                        command.Ids,
                        command.OldIngredient,
                        appendedIngredient);
            }

            _decoratee.Execute(command);
        }
    }
}
