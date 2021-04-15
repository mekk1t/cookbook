using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateIngredientDecorator : ICommand<CreateIngredientCommand>
    {
        private readonly ICommand<CreateIngredientCommand> _decoratee;

        private readonly IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> _getIngredientsQuery;
        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;

        public CreateIngredientDecorator(
            ICommand<CreateIngredientCommand> decoratee,
            IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> getIngredientsQuery,
            ICommand<CreateCategoryCommand> createCategory,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories)
        {
            _decoratee = decoratee;
            _getIngredientsQuery = getIngredientsQuery;
            _createCategory = createCategory;
            _getCategories = getCategories;
        }

        public void Execute(CreateIngredientCommand command)
        {
            var ingredientNames = _getIngredientsQuery.Execute(new GetIngredientsQuery()).Select(i => i.Name);
            if (!ingredientNames.Contains(command.Name))
            {
                var oldCategories = _getCategories.Execute(new GetCategoriesQuery()).Select(c => c.Name);
                foreach (var newCategory in command.Categories)
                {
                    if (!oldCategories.Contains(newCategory))
                    {
                        _createCategory.Execute(new CreateCategoryCommand(newCategory));
                    }
                }
                _decoratee.Execute(command);
            }
        }
    }
}
