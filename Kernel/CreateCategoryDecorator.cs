using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateCategoryDecorator
    {
        private readonly ICommand<CreateCategoryCommand> _createCategoryCommand;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategoriesQuery;

        public CreateCategoryDecorator(
            ICommand<CreateCategoryCommand> createCategoryCommand,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategoriesQuery)
        {
            _createCategoryCommand = createCategoryCommand;
            _getCategoriesQuery = getCategoriesQuery;
        }

        public void CreateCategory(CreateCategoryCommand command)
        {
            var categoriesNames = _getCategoriesQuery.Execute(new GetCategoriesQuery()).Select(c => c.Name);
            if (!categoriesNames.Contains(command.Name))
            {
                _createCategoryCommand.Execute(command);
            }
        }
    }
}
