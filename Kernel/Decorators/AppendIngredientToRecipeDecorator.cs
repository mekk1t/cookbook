using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;

namespace KitProjects.MasterChef.Kernel.Decorators
{
    public class AppendIngredientToRecipeDecorator : ICommand<AppendRecipeIngredientCommand>
    {
        private readonly ICommand<AppendRecipeIngredientCommand> _decoratee;
        private readonly IEntityChecker<Recipe, Guid> _recipeChecker;
        private readonly IEntityChecker<Ingredient, Guid> _ingredientChecker;
        private readonly ICommand<CreateIngredientCommand> _createIngredient;

        public AppendIngredientToRecipeDecorator(
            ICommand<AppendRecipeIngredientCommand> decoratee,
            IEntityChecker<Recipe, Guid> recipeChecker,
            IEntityChecker<Ingredient, Guid> ingredientChecker,
            ICommand<CreateIngredientCommand> createIngredient)
        {
            _decoratee = decoratee;
            _recipeChecker = recipeChecker;
            _ingredientChecker = ingredientChecker;
            _createIngredient = createIngredient;
        }

        public void Execute(AppendRecipeIngredientCommand command)
        {
            bool recipeExists = _recipeChecker.CheckExistence(command.RecipeId);
            if (!recipeExists)
                throw new ArgumentException(null, nameof(command));

            bool ingredientExists = _ingredientChecker.CheckExistence(command.Ingredient.Id);
            if (!ingredientExists)
                _createIngredient.Execute(new CreateIngredientCommand(command.Ingredient.Name, Array.Empty<string>()));

            _decoratee.Execute(command);
        }
    }
}
