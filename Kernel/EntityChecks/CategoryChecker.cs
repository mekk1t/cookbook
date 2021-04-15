using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class CategoryChecker : IEntityChecker<Category, string>
    {
        private readonly IQuery<Category, SearchCategoryQuery> _searchCategory;

        public CategoryChecker(IQuery<Category, SearchCategoryQuery> searchCategory)
        {
            _searchCategory = searchCategory;
        }

        public bool CheckExistence(string parameters = null)
        {
            if (parameters.IsNullOrEmpty())
                return false;

            var category = _searchCategory.Execute(new SearchCategoryQuery(parameters));
            if (category == null)
                return false;

            return true;
        }
    }
}
