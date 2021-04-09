using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Recipes
{
    public class SearchRecipeQueryHandler : IQuery<Recipe, SearchRecipeCommand>
    {
        private readonly AppDbContext _dbContext;

        public SearchRecipeQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Recipe Execute(SearchRecipeCommand query)
        {
            if (query.RecipeId != Guid.Empty)
            {
                var recipe = _dbContext.Recipes.AsNoTracking().FirstOrDefault(r => r.Id == query.RecipeId);
                if (recipe == null)
                    return null;

                return new Recipe(recipe.Id);
            }

            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                var recipe = _dbContext.Recipes.AsNoTracking().FirstOrDefault(r =>
                    r.Title == query.SearchTerm ||
                    r.Title.Contains(query.SearchTerm) ||
                    query.SearchTerm.Contains(r.Title));
                if (recipe == null)
                    return null;

                return new Recipe(recipe.Id);
            }

            throw new ArgumentException(null, nameof(query));
        }
    }
}
