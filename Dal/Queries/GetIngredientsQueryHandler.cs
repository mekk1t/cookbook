using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            {
                var ingredients = _dbContext.Ingredients
                    .AsNoTracking()
                    .Include(i => i.Categories)
                    .Include(i => i.RecipeIngredientsLink).ThenInclude(i => i.DbRecipe)
                    .OrderBy(i => i.Name)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .AsEnumerable();
                var result = ingredients.Select(i =>
                {
                    var ingredient = new Ingredient(i.Id, i.Name);
                    ingredient.Categories.AddRange(i.Categories.Select(c => new Category(c.Id, c.Name)));
                    ingredient.Recipes.AddRange(i.RecipeIngredientsLink
                        .Select(link => link.DbRecipe)
                        .Select(dbRecipe => new Recipe(dbRecipe.Id)
                        {
                            Title = dbRecipe.Title,
                            Description = dbRecipe.Description
                        }));
                    return ingredient;
                });
                return result;
            }

            return _dbContext.Ingredients
                .AsNoTracking()
                .OrderBy(i => i.Name)
                .Skip(query.Offset)
                .Take(query.Limit)
                .Select(i => new Ingredient(i.Id, i.Name))
                .AsEnumerable();
        }
    }
}
