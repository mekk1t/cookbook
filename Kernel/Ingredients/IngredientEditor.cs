using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Kernel.Ingredients
{
    public class IngredientEditor
    {
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;
        private readonly IQuery<Ingredient, SearchIngredientQuery> _searchIngredient;
        private readonly ICommand<AppendIngredientCategoryCommand> _appendCategory;
        private readonly ICommand<RemoveIngredientCategoryCommand> _removeCategory;

        public IngredientEditor(
            IQuery<Category, SearchCategoryQuery> searchCategory,
            IQuery<Ingredient, SearchIngredientQuery> searchIngredient,
            ICommand<AppendIngredientCategoryCommand> appendCategory,
            ICommand<RemoveIngredientCategoryCommand> removeCategory)
        {
            _searchCategory = searchCategory;
            _searchIngredient = searchIngredient;
            _appendCategory = appendCategory;
            _removeCategory = removeCategory;
        }

    }
}
