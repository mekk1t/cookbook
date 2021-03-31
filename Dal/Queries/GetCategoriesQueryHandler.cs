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
            {
                var categories = _dbContext.Categories.AsNoTracking()
                    .Include(c => c.Ingredients)
                    .Include(c => c.RecipesCategoriesLink).ThenInclude(c => c.DbRecipe)
                    .OrderBy(c => c.Name)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .ToList();
                var result = categories.Select(c =>
                {
                    var category = new Category(c.Id, c.Name);
                    category.Ingredients.AddRange(c.Ingredients.Select(i => new Ingredient(i.Id, i.Name)));
                    category.Recipes.AddRange(c.RecipesCategoriesLink.Select(link => link.DbRecipe)
                        .Select(c => new Recipe(c.Id)
                        {
                            Title = c.Title,
                            Description = c.Description
                        }));
                    return category;
                });

                return result;
            }

            return _dbContext.Categories.AsNoTracking()
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .Select(c => new Category(c.Id, c.Name))
                    .ToList();
        }
    }
}
