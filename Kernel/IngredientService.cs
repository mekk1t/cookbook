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
        private readonly ICommand<DeleteIngredientCommand> _deleteIngredientCommand;
        private readonly ICommand<EditIngredientCommand> _editIngredientCommand;
        private readonly ICommand<CreateIngredientCommand> _createIngredientCommand;
        private readonly IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> _getIngredientsQuery;
        private readonly CategoryService _categoryService;

        public IngredientService(
            ICommand<CreateIngredientCommand> createIngredientCommand,
            IQuery<IEnumerable<Ingredient>, GetIngredientsQuery> getIngredientsQuery, CategoryService categoryService,
            ICommand<EditIngredientCommand> editIngredientCommand,
            ICommand<DeleteIngredientCommand> deleteIngredientCommand)
        {
            _createIngredientCommand = createIngredientCommand;
            _getIngredientsQuery = getIngredientsQuery;
            _categoryService = categoryService;
            _editIngredientCommand = editIngredientCommand;
            _deleteIngredientCommand = deleteIngredientCommand;
        }

        public void CreateIngredient(CreateIngredientCommand command)
        {
            var ingredientNames = _getIngredientsQuery.Execute(new GetIngredientsQuery()).Select(i => i.Name);
            if (!ingredientNames.Contains(command.Name))
            {
                var oldCategories = _categoryService.GetCategories().Select(c => c.Name);
                foreach (var newCategory in command.Categories)
                {
                    if (!oldCategories.Contains(newCategory))
                    {
                        _categoryService.CreateCategory(new CreateCategoryCommand(newCategory));
                    }
                }
                _createIngredientCommand.Execute(command);
            }
        }

        public IEnumerable<Ingredient> GetIngredients(GetIngredientsQuery query) => _getIngredientsQuery.Execute(query);
        public void EditIngredient(EditIngredientCommand command) => _editIngredientCommand.Execute(command);
        public void DeleteIngredient(DeleteIngredientCommand command) => _deleteIngredientCommand.Execute(command);
    }
}
