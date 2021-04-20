using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Categories
{
    public class SearchCategoryQueryHandler : IQuery<Category, SearchCategoryQuery>
    {
        private readonly AppDbContext _dbContext;

        public SearchCategoryQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category Execute(SearchCategoryQuery query)
        {
            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                return _dbContext.Categories
                    .AsNoTracking()
                    .Where(c =>
                        c.Name == query.SearchTerm ||
                        c.Name.Contains(query.SearchTerm) ||
                        query.SearchTerm.Contains(c.Name))
                    .Select(c => new Category(c.Id, c.Name))
                    .FirstOrDefault();
            }

            throw new ArgumentException(null, nameof(query));
        }
    }
}
