using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetIngredientsQueryHandler : IQuery<IEnumerable<Ingredient>>
    {
        private readonly AppDbContext _dbContext;

        public GetIngredientsQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Ingredient> Execute()
        {
            return _dbContext.Ingredients
                .AsNoTracking()
                .Select(i => new Ingredient(i.Id, i.Name, new Collection<Category>()))
                .AsEnumerable();
        }
    }
}
