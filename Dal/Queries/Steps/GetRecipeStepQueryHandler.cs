using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Queries.Get;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Steps
{
    public class GetRecipeStepQueryHandler : IQuery<RecipeStep, GetRecipeStepQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetRecipeStepQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RecipeStep Execute(GetRecipeStepQuery query)
        {
            IQueryable<DbRecipe> dbRecipes = _dbContext.Recipes.AsNoTracking();

            var step = dbRecipes
                .Where(recipe => recipe.Id == query.RecipeId)
                .Where(r => r.Steps.Select(s => s.Id).Contains(query.StepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == query.StepId))
                ?.FirstOrDefault();
            if (step == null)
                return null;

            return new RecipeStep(step.Id);
        }
    }
}
