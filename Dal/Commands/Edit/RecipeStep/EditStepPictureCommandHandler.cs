using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class EditStepPictureCommandHandler : ICommand<EditStepPictureCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditStepPictureCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditStepPictureCommand command)
        {
            var step = _dbContext.Recipes
                .Where(r => r.Steps.Select(s => s.Id).Contains(command.StepId))
                .Select(r => r.Steps.FirstOrDefault(step => step.Id == command.StepId))
                ?.FirstOrDefault();
            if (step == null)
                throw new ArgumentException(null, nameof(command));

            step.Image = command.NewImage;
            _dbContext.SaveChanges();
        }
    }
}
