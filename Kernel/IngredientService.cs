using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Kernel
{
    public class IngredientService
    {
        private readonly ICommand<CreateIngredientCommand> _createIngredientCommand;
        private readonly IQuery<IEnumerable<Ingredient>> _getIngredientsQuery;
        private readonly CategoryService _categoryService;

        public IngredientService()
        {

        }

        public void CreateIngredient(CreateIngredientCommand command)
        {
            var ingredientNames = _getIngredientsQuery.Execute().Select(i => i.Name);
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
    }
}
