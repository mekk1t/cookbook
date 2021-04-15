using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class IngredientService
    {
        private readonly ICommand<CreateIngredientCommand> _createIngredientCommand;
        private readonly IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> _getIngredientsQuery;
        private readonly CreateCategoryDecorator _categoryService;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;

        public IngredientService(
            ICommand<CreateIngredientCommand> createIngredientCommand,
            IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> getIngredientsQuery,
            CreateCategoryDecorator categoryService,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories)
        {
            _createIngredientCommand = createIngredientCommand;
            _getIngredientsQuery = getIngredientsQuery;
            _categoryService = categoryService;
            _getCategories = getCategories;
        }

        public void CreateIngredient(CreateIngredientCommand command)
        {
            var ingredientNames = _getIngredientsQuery.Execute(new GetIngredientsQuery()).Select(i => i.Name);
            if (!ingredientNames.Contains(command.Name))
            {
                var oldCategories = _getCategories.Execute(new GetCategoriesQuery()).Select(c => c.Name);
                foreach (var newCategory in command.Categories)
                {
                    if (!oldCategories.Contains(newCategory))
                    {
                        _categoryService.Execute(new CreateCategoryCommand(newCategory));
                    }
                }
                _createIngredientCommand.Execute(command);
            }
        }
    }
}
