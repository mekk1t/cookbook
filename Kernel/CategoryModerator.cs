using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CategoryModerator
    {
        private readonly ICommand<CreateCategoryCommand> _createCategoryCommand;
        private readonly IQuery<IEnumerable<Category>> _getCategoriesQuery;

        public CategoryModerator(
            ICommand<CreateCategoryCommand> createCategoryCommand,
            IQuery<IEnumerable<Category>> getCategoriesQuery)
        {
            _createCategoryCommand = createCategoryCommand;
            _getCategoriesQuery = getCategoriesQuery;
        }

        public void CreateCategory(CreateCategoryCommand command)
        {
            var categoriesNames = _getCategoriesQuery.Execute().Select(c => c.Name);
            if (!categoriesNames.Contains(command.Name))
            {
                _createCategoryCommand.Execute(command);
            }
        }

        public IEnumerable<Category> GetCategories() => _getCategoriesQuery.Execute();
    }
}
