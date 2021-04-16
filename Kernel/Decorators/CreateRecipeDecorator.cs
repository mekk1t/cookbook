using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateRecipeDecorator : ICommand<CreateRecipeCommand>
    {
        private readonly ICommand<CreateRecipeCommand> _decoratee;

        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly ICommand<CreateIngredientCommand> _createIngredient;

        public CreateRecipeDecorator(
            ICommand<CreateRecipeCommand> decoratee,
            ICommand<CreateCategoryCommand> createCategory,
            ICommand<CreateIngredientCommand> createIngredient)
        {
            _decoratee = decoratee;
            _createCategory = createCategory;
            _createIngredient = createIngredient;
        }

        public void Execute(CreateRecipeCommand command)
        {
            if (command.IngredientsDetails?.Count() > 0)
            {
                foreach (var ingredient in command.IngredientsDetails)
                {
                    _createIngredient.Execute(new CreateIngredientCommand(ingredient.IngredientName, new List<string>()));
                }
            }

            if (command.Categories?.Count() > 0)
            {
                foreach (var category in command.Categories)
                {
                    _createCategory.Execute(new CreateCategoryCommand(category));
                }
            }

            _decoratee.Execute(command);
        }
    }
}
