using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class DeleteRecipeCommandHandler : ICommand<DeleteRecipeCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteRecipeCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(DeleteRecipeCommand command)
        {
            var oldRecipe = _dbContext.Recipes
                .Include(r => r.Steps)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            var steps = oldRecipe.Steps;
            if (oldRecipe == null)
                return;
            _dbContext.Recipes.Remove(oldRecipe);
            _dbContext.SaveChanges();
            _dbContext.RemoveRange(steps);
            _dbContext.SaveChanges();
        }
    }
}
