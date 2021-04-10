using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class NormalizeStepsOrderCommandHandler : ICommand<NormalizeStepsOrderCommand>
    {
        private readonly AppDbContext _dbContext;

        public NormalizeStepsOrderCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(NormalizeStepsOrderCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var steps = recipe.Steps
                .OrderBy(step => step.Index)
                .ToList();
            for (int i = command.StartIndex; i < steps.Count; i++)
            {
                steps[i].Index = i + 1;
            }

            _dbContext.SaveChanges();
        }
    }
}
