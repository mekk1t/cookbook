using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetCategoriesQueryHandler : IQuery<IEnumerable<Category>, GetCategoriesQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetCategoriesQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> Execute(GetCategoriesQuery query)
        {
            if (query.WithRelationships)
                return _dbContext.Categories.AsNoTracking()
                    .Include(c => c.Ingredients)
                    .OrderBy(c => c.Name)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .Select(c => new Category(c.Id, c.Name, c.Ingredients.Select(i => new Ingredient(i.Id, i.Name))))
                    .ToList();

            return _dbContext.Categories.AsNoTracking()
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .Select(c => new Category(c.Id, c.Name))
                    .ToList();
        }
    }
}
