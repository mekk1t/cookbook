using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateIngredientDecorator : ICommand<CreateIngredientCommand>
    {
        private readonly ICommand<CreateIngredientCommand> _decoratee;

        private readonly IEntityChecker<Ingredient, string> _ingredientChecker;
        private readonly ICommand<CreateCategoryCommand> _createCategory;

        public CreateIngredientDecorator(
            ICommand<CreateIngredientCommand> decoratee,
            IEntityChecker<Ingredient, string> ingredientChecker,
            ICommand<CreateCategoryCommand> createCategory)
        {
            _decoratee = decoratee;
            _ingredientChecker = ingredientChecker;
            _createCategory = createCategory;
        }

        public void Execute(CreateIngredientCommand command)
        {
            var ingredientExists = _ingredientChecker.CheckExistence(command.Name);
            if (!ingredientExists)
            {
                foreach (var newCategory in command.Categories)
                {
                    _createCategory.Execute(new CreateCategoryCommand(newCategory));
                }
                _decoratee.Execute(command);
            }
        }
    }
}
