using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class RemoveRecipeIngredientCommandHandler : ICommand<RemoveRecipeIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveRecipeIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(RemoveRecipeIngredientCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeIngredientLink)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var oldIngredientLink = recipe.RecipeIngredientLink.FirstOrDefault(r => r.DbIngredientId == command.IngredientId);
            if (oldIngredientLink == null)
                return;

            recipe.RecipeIngredientLink.Remove(oldIngredientLink);
            _dbContext.SaveChanges();
        }
    }
}
