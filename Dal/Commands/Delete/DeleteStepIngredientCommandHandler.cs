using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Delete
{
    public class DeleteStepIngredientCommandHandler : ICommand<DeleteIngredientFromStepCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteStepIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(DeleteIngredientFromStepCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink)
                .FirstOrDefault(r => r.Id == command.Ids.Recipe);
            if (recipe == null)
                throw new EntityNotFoundException(command.Ids.Recipe);
            var step = recipe.Steps
                .FirstOrDefault(r => r.Id == command.Ids.Step);
            if (step == null)
                throw new EntityNotFoundException(command.Ids.Step);

            var ingredientToRemove = step.StepIngredientsLink.FirstOrDefault(link => link.DbIngredientId == command.IngredientId);
            if (ingredientToRemove == null)
                throw new InvalidOperationException($"Такого ингредиента не существует в шаге с ID {command.Ids.Step}");

            step.StepIngredientsLink.Remove(ingredientToRemove);

            _dbContext.SaveChanges();
        }
    }
}
