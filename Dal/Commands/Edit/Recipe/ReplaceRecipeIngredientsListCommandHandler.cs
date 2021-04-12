using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class ReplaceRecipeIngredientsListCommandHandler : ICommand<ReplaceIngredientsListCommand>
    {
        private readonly AppDbContext _dbContext;

        public ReplaceRecipeIngredientsListCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(ReplaceIngredientsListCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeIngredientLink)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var newIngredients = _dbContext.Ingredients
                .Where(i => command.NewIngredients.Select(c => c.Id).Contains(i.Id))
                .ToList();

            recipe.RecipeIngredientLink.Clear();
            foreach (var newIngredient in newIngredients)
            {
                recipe.RecipeIngredientLink.Add(new DbRecipeIngredient
                {
                    DbRecipe = recipe,
                    DbIngredient = newIngredient
                });
            }

            _dbContext.SaveChanges();
        }
    }
}
