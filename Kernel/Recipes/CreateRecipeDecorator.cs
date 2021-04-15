using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Kernel
{
    public class CreateRecipeDecorator : ICommand<CreateRecipeCommand>
    {
        private readonly ICommand<CreateRecipeCommand> _decoratee;

        private readonly ICommand<CreateCategoryCommand> _createCategory;
        private readonly ICommand<CreateIngredientCommand> _createIngredient;
        private readonly IQuery<IEnumerable<Category>, GetCategoriesQuery> _getCategories;

        public CreateRecipeDecorator(
            ICommand<CreateRecipeCommand> decoratee,
            ICommand<CreateCategoryCommand> createCategory,
            ICommand<CreateIngredientCommand> createIngredient,
            IQuery<IEnumerable<Category>, GetCategoriesQuery> getCategories)
        {
            _decoratee = decoratee;
            _createCategory = createCategory;
            _createIngredient = createIngredient;
            _getCategories = getCategories;
        }

        public void Execute(CreateRecipeCommand command)
        {
            if (command.IngredientsDetails?.Count() > 0)
            {
                foreach (var ingredient in command.IngredientsDetails)
                {
                    _createIngredient.Execute(new CreateIngredientCommand(ingredient.IngredientName, new List<string>()));
                }
            }

            if (command.Categories?.Count() > 0)
            {
                foreach (var category in command.Categories)
                {
                    var oldCategory = _getCategories.Execute(new GetCategoriesQuery(limit: 1000)).FirstOrDefault(c => c.Name == category);
                    if (oldCategory == null)
                    {
                        _createCategory.Execute(new CreateCategoryCommand(category));
                    }
                }
            }

            _decoratee.Execute(command);
        }
    }
}
