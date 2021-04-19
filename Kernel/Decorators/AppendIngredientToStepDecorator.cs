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
    public class AppendIngredientToStepDecorator : ICommand<AppendIngredientToStepCommand>
    {
        private readonly ICommand<AppendIngredientToStepCommand> _decoratee;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;
        private readonly ICommand<AppendRecipeIngredientCommand> _appendIngredientToRecipe;
        private readonly IQuery<Ingredient, GetIngredientQuery> _getIngredient;

        public AppendIngredientToStepDecorator(
            ICommand<AppendIngredientToStepCommand> decoratee,
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe,
            ICommand<AppendRecipeIngredientCommand> appendIngredientToRecipe,
            IQuery<Ingredient, GetIngredientQuery> getIngredient)
        {
            _decoratee = decoratee;
            _getRecipe = getRecipe;
            _appendIngredientToRecipe = appendIngredientToRecipe;
            _getIngredient = getIngredient;
        }

        public void Execute(AppendIngredientToStepCommand command)
        {
            var recipe = _getRecipe.Execute(new GetRecipeQuery(command.Ids.Recipe));
            if (recipe == null)
                throw new EntityNotFoundException(command.Ids.Recipe);
            var step = recipe.Steps.FirstOrDefault(step => step.Id == command.Ids.Step);
            if (step == null)
                throw new EntityNotFoundException(typeof(RecipeStep), command.Ids.Step);

            if (!recipe.Ingredients.Select(i => i.IngredientName).Contains(command.Ingredient.Name))
            {
                _appendIngredientToRecipe.Execute(
                    new AppendRecipeIngredientCommand(
                        command.Ids.Recipe,
                        command.Ingredient,
                        command.Parameters));
                var createdIngredient = _getIngredient.Execute(new GetIngredientQuery(command.Ingredient.Name));
                command = new AppendIngredientToStepCommand(command.Ids, createdIngredient, command.Parameters);
            }

            _decoratee.Execute(command);
        }
    }
}
