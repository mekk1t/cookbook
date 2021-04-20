using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Steps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class ReplaceStepIngredientCommandHandler : ICommand<ReplaceStepIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public ReplaceStepIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(ReplaceStepIngredientCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink).ThenInclude(link => link.DbIngredient)
                .FirstOrDefault(r => r.Id == command.Ids.Recipe);
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var step = recipe.Steps.FirstOrDefault(step => step.Id == command.Ids.Step);
            if (step == null)
                throw new ArgumentException(null, nameof(command));

            var oldIngredient = step.StepIngredientsLink.FirstOrDefault(link => link.DbIngredientId == command.OldIngredient.Id);
            if (oldIngredient == null)
                throw new ArgumentException(null, nameof(command));

            step.StepIngredientsLink.Remove(oldIngredient);
            step.StepIngredientsLink.Add(new Database.Models.DbRecipeStepIngredient
            {
                DbRecipeStepId = step.Id,
                DbIngredientId = command.NewIngredient.Id,
                Amount = oldIngredient.Amount,
                Measure = oldIngredient.Measure
            });
            _dbContext.SaveChanges();
        }
    }
}
