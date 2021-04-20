using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class EditStepIngredientDescriptionCommandHandler : ICommand<EditStepIngredientDescriptionCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditStepIngredientDescriptionCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditStepIngredientDescriptionCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink)
                .FirstOrDefault(r => r.Id == command.Ids.Recipe);
            if (recipe == null)
                throw new EntityNotFoundException(command.Ids.Recipe);
            var step = recipe.Steps.FirstOrDefault(r => r.Id == command.Ids.Step);
            if (step == null)
                throw new EntityNotFoundException(command.Ids.Step);

            var editIngredientLink = step.StepIngredientsLink.FirstOrDefault(link => link.DbIngredientId == command.IngredientId);
            if (editIngredientLink == null)
                throw new EntityNotFoundException(command.IngredientId);

            if (command.Amount != default)
                editIngredientLink.Amount = command.Amount;
            editIngredientLink.Measure = command.Measure;

            _dbContext.SaveChanges();
        }
    }
}
