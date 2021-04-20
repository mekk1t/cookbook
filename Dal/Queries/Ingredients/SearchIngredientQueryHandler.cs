using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Ingredients
{
    public class SearchIngredientQueryHandler : IQuery<Ingredient, SearchIngredientQuery>
    {
        private readonly AppDbContext _dbContext;

        public SearchIngredientQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Ingredient Execute(SearchIngredientQuery query)
        {
            if (query.IngredientId != Guid.Empty)
            {
                var ingredient = _dbContext.Ingredients
                    .AsNoTracking()
                    .Include(i => i.Categories)
                    .FirstOrDefault(i => i.Id == query.IngredientId);
                if (ingredient == null)
                    return null;

                var result = new Ingredient(ingredient.Id, ingredient.Name);
                result.Categories.AddRange(ingredient.Categories.Select(c => new Category(c.Id, c.Name)));
                return result;
            }

            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                var ingredient = _dbContext.Ingredients
                    .AsNoTracking()
                    .Where(i => i.Name == query.SearchTerm || i.Name.Contains(query.SearchTerm) || query.SearchTerm.Contains(i.Name))
                    .Select(i => new Ingredient(i.Id, i.Name))
                    .FirstOrDefault();
                if (ingredient == null)
                    return null;

                var result = new Ingredient(ingredient.Id, ingredient.Name);
                result.Categories.AddRange(ingredient.Categories.Select(c => new Category(c.Id, c.Name)));
                return result;
            }

            throw new ArgumentException(null, nameof(query));
        }
    }
}
