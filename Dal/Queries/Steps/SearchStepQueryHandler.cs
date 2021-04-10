using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Steps
{
    public class SearchStepQueryHandler : IQuery<RecipeStep, SearchStepQuery>
    {
        private readonly AppDbContext _dbContext;

        public SearchStepQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RecipeStep Execute(SearchStepQuery query)
        {
            if (query.Parameters != null)
            {
                if (query.Parameters.RecipeId != Guid.Empty && query.Parameters.Index > 0)
                {
                    var dbStep  = _dbContext.Recipes
                        .AsNoTracking()
                        .FirstOrDefault(r => r.Id == query.Parameters.RecipeId)
                        ?.Steps
                        .FirstOrDefault(step => step.Index == query.Parameters.Index);
                    if (dbStep == null)
                        return null;

                    return new RecipeStep(dbStep.Id);
                }
            }

            var step = _dbContext.Recipes
                .AsNoTracking()
                .Where(r => r.Steps.Select(s => s.Id).Contains(query.StepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == query.StepId))
                ?.FirstOrDefault();
            if (step == null)
                return null;

            return new RecipeStep(step.Id);
        }
    }
}
