using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class GetCategoriesResponse
    {
        public IEnumerable<string> CategoryNames { get; }

        public GetCategoriesResponse(IEnumerable<Category> categories)
        {
            this.CategoryNames = categories.Select(c => c.Name);
        }
    }
}
