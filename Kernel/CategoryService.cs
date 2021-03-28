﻿using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CategoryService
    {
        private readonly ICommand<EditCategoryCommand> _editCategoryCommand;
        private readonly ICommand<DeleteCategoryCommand> _deleteCategoryCommand;
        private readonly ICommand<CreateCategoryCommand> _createCategoryCommand;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategoriesQuery;

        public CategoryService(
            ICommand<CreateCategoryCommand> createCategoryCommand,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategoriesQuery,
            ICommand<DeleteCategoryCommand> deleteCategoryCommand,
            ICommand<EditCategoryCommand> editCategoryCommand)
        {
            _createCategoryCommand = createCategoryCommand;
            _getCategoriesQuery = getCategoriesQuery;
            _deleteCategoryCommand = deleteCategoryCommand;
            _editCategoryCommand = editCategoryCommand;
        }

        public void CreateCategory(CreateCategoryCommand command)
        {
            var categoriesNames = _getCategoriesQuery.Execute(new GetCategoriesQuery()).Select(c => c.Name);
            if (!categoriesNames.Contains(command.Name))
            {
                _createCategoryCommand.Execute(command);
            }
        }

        public void EditCategory(EditCategoryCommand command) => _editCategoryCommand.Execute(command);
        public void DeleteCategory(DeleteCategoryCommand command) => _deleteCategoryCommand.Execute(command);
        public IEnumerable<Category> GetCategories(GetCategoriesQuery query) => _getCategoriesQuery.Execute(query);
    }
}
