using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class SwapStepsCommandHandler : ICommand<SwapStepsCommand>
    {
        private readonly AppDbContext _dbContext;

        public SwapStepsCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(SwapStepsCommand command)
        {
            var firstStep  = _dbContext.Recipes
                .Where(r => r.Steps.Select(s => s.Id).Contains(command.FirstStepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == command.FirstStepId))
                ?.FirstOrDefault();
            if (firstStep == null)
                throw new ArgumentException(null, nameof(command));

            var secondStep = _dbContext.Recipes
                .Where(r => r.Steps.Select(s => s.Id).Contains(command.SecondStepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == command.SecondStepId))
                ?.FirstOrDefault();
            if (secondStep == null)
                throw new ArgumentException(null, nameof(command));

            int temp = secondStep.Index;
            secondStep.Index = firstStep.Index;
            firstStep.Index = temp;

            _dbContext.SaveChanges();
        }
    }
}
