using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetCategoriesQuery : IQuery<IEnumerable<Category>>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoriesQuery(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> Execute()
        {
            return _dbContext.Categories.AsNoTracking();
        }
    }
}
