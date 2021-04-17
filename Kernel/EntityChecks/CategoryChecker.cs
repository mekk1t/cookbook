using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;

namespace KitProjects.MasterChef.Kernel.EntityChecks
{
    public class CategoryChecker : IEntityChecker<Category, string>
    {
        private readonly IQuery<Category, GetCategoryQuery> _getCategory;

        public CategoryChecker(IQuery<Category, GetCategoryQuery> getCategory)
        {
            _getCategory = getCategory;
        }

        public bool CheckExistence(string parameters = null)
        {
            if (parameters.IsNullOrEmpty())
                return false;

            var category = _getCategory.Execute(new GetCategoryQuery(parameters));
            if (category == null)
                return false;

            return true;
        }
    }
}
