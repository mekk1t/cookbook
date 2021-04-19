using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class AppendIngredientCommandHandler : ICommand<AppendRecipeIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendRecipeIngredientCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeIngredientLink)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException(command.RecipeId.ToString());

            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Name == command.Ingredient.Name);
            if (ingredient == null)
                throw new EntityNotFoundException(command.Ingredient.Name);

            recipe.RecipeIngredientLink.Add(new DbRecipeIngredient
            {
                DbIngredient = ingredient,
                DbRecipe = recipe,
                IngredientMeasure = command.Parameters.Measure,
                IngredientxAmount = command.Parameters.Amount,
                Notes = command.Parameters.Notes
            });

            _dbContext.SaveChanges();
        }
    }
}
