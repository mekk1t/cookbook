using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class AppendRecipeStepCommandHandler : ICommand<AppendRecipeStepCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendRecipeStepCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendRecipeStepCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new ArgumentException(null, nameof(command));

            var newStep = command.Step;
            var appendStep = new DbRecipeStep
            {
                Id = newStep.Id,
                Description = newStep.Description,
                Index = recipe.Steps.Count > 0
                    ? recipe.Steps.OrderBy(step => step.Index).Last().Index + 1
                    : 1,
                Image = newStep.Image,
                StepIngredientsLink = new List<DbRecipeStepIngredient>()
            };

            if (newStep.IngredientsDetails.Count > 0)
            {
                foreach (var details in newStep.IngredientsDetails)
                {
                    var ingredientId = _dbContext.Ingredients
                        .AsNoTracking()
                        .FirstOrDefault(i => i.Name == details.IngredientName)
                        ?.Id;
                    if (ingredientId == null)
                        continue;

                    appendStep.StepIngredientsLink.Add(new DbRecipeStepIngredient
                    {
                        DbRecipeStepId = appendStep.Id,
                        DbIngredientId = ingredientId.Value,
                        Amount = details.Amount,
                        Measure = details.Measure
                    });
                }
            }

            recipe.Steps.Add(appendStep);
            _dbContext.SaveChanges();
        }
    }
}
