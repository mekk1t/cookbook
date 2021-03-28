using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetIngredientsQueryHandler : IQuery<IEnumerable<Ingredient>, GetIngredientsQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetIngredientsQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Ingredient> Execute(GetIngredientsQuery query)
        {
            if (query.WithRelationships)
                return _dbContext.Ingredients
                    .AsNoTracking()
                    .Include(i => i.Categories)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .Select(i => new Ingredient(i.Id, i.Name, i.Categories
                        .Select(c => new Category(c.Id, c.Name))
                        .ToList()))
                    .AsEnumerable();

            return _dbContext.Ingredients
                .AsNoTracking()
                .Skip(query.Offset)
                .Take(query.Limit)
                .Select(i => new Ingredient(i.Id, i.Name, new Collection<Category>()))
                .AsEnumerable();
        }
    }
}
