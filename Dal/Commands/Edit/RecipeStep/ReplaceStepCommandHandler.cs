using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Steps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class ReplaceStepCommandHandler : ICommand<ReplaceStepCommand>
    {
        private readonly AppDbContext _dbContext;

        public ReplaceStepCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(ReplaceStepCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink).ThenInclude(link => link.DbIngredient)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var oldStep = recipe.Steps.FirstOrDefault(step => step.Id == command.StepId);
            if (oldStep == null)
                throw new InvalidOperationException();

            var existingIngredients = _dbContext.Ingredients
                .AsNoTracking()
                .Where(i => command.Ingredients.Select(i => i.IngredientName).Contains(i.Name))
                .ToList();
            ICollection<DbRecipeStepIngredient> stepIngredients = new Collection<DbRecipeStepIngredient>();
            var stepId = Guid.NewGuid();
            foreach (var existingIngredient in existingIngredients)
            {
                var newStepIngredientDetails = command.Ingredients.First(i => i.IngredientName == existingIngredient.Name);
                stepIngredients.Add(new DbRecipeStepIngredient
                {
                    DbRecipeStepId = stepId,
                    DbIngredientId = existingIngredient.Id,
                    Amount = newStepIngredientDetails.Amount,
                    Measure = newStepIngredientDetails.Measure
                });
            }

            recipe.Steps.Remove(oldStep);
            recipe.Steps.Add(new DbRecipeStep
            {
                Id = stepId,
                Description = command.Description,
                Image = command.Image,
                Index = oldStep.Index,
                StepIngredientsLink = stepIngredients
            });
            _dbContext.SaveChanges();
        }
    }
}
