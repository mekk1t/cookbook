using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class CategoryCrud
    {
        private readonly ICommand<DeleteCategoryCommand> _deleteCategory;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;
        private readonly IQuery<Category, GetCategoryQuery> _getCategory;
        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly ICommand<EditCategoryCommand> _editCategory;

        public CategoryCrud(
            ICommand<DeleteCategoryCommand> deleteCategory,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories,
            IQuery<Category, GetCategoryQuery> getCategory,
            ICommand<CreateCategoryCommand> createCategory,
            ICommand<EditCategoryCommand> editCategory)
        {
            _deleteCategory = deleteCategory;
            _getCategories = getCategories;
            _getCategory = getCategory;
            _createCategory = createCategory;
            _editCategory = editCategory;
        }

        public void Create(string name) => _createCategory.Execute(new CreateCategoryCommand(name));

        public Category Read(Guid id) => _getCategory.Execute(new GetCategoryQuery(id));

        public Category Read(string name) => _getCategory.Execute(new GetCategoryQuery(name));

        public IEnumerable<Category> Read() => _getCategories.Execute(new GetCategoriesQuery());

        public IEnumerable<Category> Read(int limit, int offset) =>
            _getCategories.Execute(new GetCategoriesQuery(limit: limit, offset: offset));

        public void Update(Guid id, string newName) => _editCategory.Execute(new EditCategoryCommand(id, newName));

        public void Delete(Guid id) => _deleteCategory.Execute(new DeleteCategoryCommand(id));
    }
}
