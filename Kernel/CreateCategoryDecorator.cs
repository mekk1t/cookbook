using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateCategoryDecorator : ICommand<CreateCategoryCommand>
    {
        private readonly ICommand<CreateCategoryCommand> _createCategoryCommand;
        private readonly IEntityChecker<Category, string> _categoryChecker;

        public CreateCategoryDecorator(
            ICommand<CreateCategoryCommand> createCategoryCommand,
            IEntityChecker<Category, string> categoryChecker)
        {
            _createCategoryCommand = createCategoryCommand;
            _categoryChecker = categoryChecker;
        }

        public void Execute(CreateCategoryCommand command)
        {
            var categoryExists = _categoryChecker.CheckExistence(command.Name);

            if (!categoryExists)
            {
                _createCategoryCommand.Execute(command);
            }
        }
    }
}
