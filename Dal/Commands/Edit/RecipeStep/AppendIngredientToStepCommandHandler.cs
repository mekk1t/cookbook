using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep
{
    public class AppendIngredientToStepCommandHandler : ICommand<AppendIngredientToStepCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendIngredientToStepCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendIngredientToStepCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink)
                .FirstOrDefault(r => r.Id == command.Ids.Recipe);
            if (recipe == null)
                throw new EntityNotFoundException(typeof(DbRecipe), command.Ids.Recipe);
            var step = recipe.Steps.FirstOrDefault(step => step.Id == command.Ids.Step);
            if (step == null)
                throw new EntityNotFoundException(typeof(DbRecipeStep), command.Ids.Recipe);

            step.StepIngredientsLink.Add(new DbRecipeStepIngredient
            {
                DbRecipeStepId = command.Ids.Step,
                DbIngredientId = command.Ingredient.Id,
                Amount = command.Parameters.Amount,
                Measure = command.Parameters.Measure
            });

            _dbContext.SaveChanges();
        }
    }
}
