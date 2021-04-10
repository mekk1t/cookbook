using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class RemoveRecipeStepCommandHandler : ICommand<RemoveRecipeStepCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveRecipeStepCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(RemoveRecipeStepCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));
            var stepToRemove = recipe.Steps.FirstOrDefault(step => step.Id == command.StepId);
            if (stepToRemove == null)
                return;

            recipe.Steps.Remove(stepToRemove);
            _dbContext.SaveChanges();
        }
    }
}
