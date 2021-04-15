using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Categories
{
    public class GetCategoryQueryHandler : IQuery<Category, GetCategoryQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoryQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category Execute(GetCategoryQuery query)
        {
            DbCategory category;
            if (query.CategoryId != Guid.Empty)
            {
                category = _dbContext.Categories.AsNoTracking().FirstOrDefault(i => i.Id == query.CategoryId);
            }
            else if (query.CategoryName.IsNotNullOrEmpty())
            {
                category = _dbContext.Categories.AsNoTracking().FirstOrDefault(i => i.Name == query.CategoryName);
            }
            else
            {
                throw new ArgumentException(null, nameof(query));
            }

            if (category == null)
                return null;

            return new Category(category.Id, category.Name);
        }
    }
}
