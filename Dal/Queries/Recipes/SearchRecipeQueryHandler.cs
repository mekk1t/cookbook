using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Recipes
{
    public class SearchRecipeQueryHandler : IQuery<Recipe, SearchRecipeQuery>
    {
        private readonly AppDbContext _dbContext;

        public SearchRecipeQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Recipe Execute(SearchRecipeQuery query)
        {
            if (query.RecipeId != Guid.Empty)
            {
                var recipe = _dbContext.Recipes
                    .AsNoTracking()
                    .Include(r => r.Steps)
                    .Include(r => r.RecipeCategoriesLink).ThenInclude(r => r.DbCategory)
                    .FirstOrDefault(r => r.Id == query.RecipeId);
                if (recipe == null)
                    return null;

                var result = new Recipe(recipe.Id);
                result.Categories.AddRange(
                    recipe.RecipeCategoriesLink.Select(link => new Category(link.DbCategory.Id, link.DbCategory.Name)));
                result.Steps.AddRange(recipe.Steps.Select(step => new RecipeStep(step.Id)
                {
                    Index = step.Index
                }));

                return result;
            }

            if (query.SearchTerm.IsNotNullOrEmpty())
            {
                var recipe = _dbContext.Recipes
                    .AsNoTracking()
                    .Include(r => r.Steps)
                    .Include(r => r.RecipeCategoriesLink).ThenInclude(r => r.DbCategory)
                    .FirstOrDefault(r =>
                    r.Title == query.SearchTerm ||
                    r.Title.Contains(query.SearchTerm) ||
                    query.SearchTerm.Contains(r.Title));
                if (recipe == null)
                    return null;

                var result = new Recipe(recipe.Id);
                result.Categories.AddRange(
                    recipe.RecipeCategoriesLink.Select(link => new Category(link.DbCategory.Id, link.DbCategory.Name)));
                result.Steps.AddRange(recipe.Steps.Select(step => new RecipeStep(step.Id)
                {
                    Index = step.Index
                }));

                return result;
            }

            throw new ArgumentException(null, nameof(query));
        }
    }
}
