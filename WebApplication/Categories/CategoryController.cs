using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    [Route("categories")]
    public class CategoryController : ApiControllerBase
    {
        [HttpGet("")]
        public CategoryViewModel GetCategories([FromServices] IQuery<IEnumerable<Category>> getAllCategoriesQuery)
        {
            var categories = getAllCategoriesQuery.Execute();
            return new CategoryViewModel(categories);
        }
    }
}
