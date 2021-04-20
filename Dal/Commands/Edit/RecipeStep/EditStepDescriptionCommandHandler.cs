using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class EditStepDescriptionCommandHandler : ICommand<EditStepDescriptionCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditStepDescriptionCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditStepDescriptionCommand command)
        {
            var step = _dbContext.Recipes
                .Where(r => r.Steps.Select(s => s.Id).Contains(command.StepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == command.StepId))
                ?.FirstOrDefault();
            if (step == null)
                throw new ArgumentException(null, nameof(command));

            step.Description = command.NewDescription;
            _dbContext.SaveChanges();
        }
    }
}
