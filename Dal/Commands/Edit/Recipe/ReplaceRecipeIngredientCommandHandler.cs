using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class ReplaceRecipeIngredientCommandHandler : ICommand<ReplaceRecipeIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public ReplaceRecipeIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(ReplaceRecipeIngredientCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeIngredientLink)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var newIngredient = _dbContext.Ingredients.FirstOrDefault(i => i.Name == command.NewIngredient.Name);
            if (newIngredient == null)
                throw new EntityNotFoundException();

            var oldIngredient = recipe.RecipeIngredientLink.FirstOrDefault(link => link.DbIngredientId == command.OldIngredient.Id);
            if (oldIngredient == null)
                return;

            recipe.RecipeIngredientLink.Remove(oldIngredient);
            recipe.RecipeIngredientLink.Add(new DbRecipeIngredient
            {
                DbRecipe = recipe,
                DbIngredient = newIngredient
            });

            _dbContext.SaveChanges();
        }
    }
}
