using KP.Cookbook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KP.Cookbook.Database
{
    public class RecipeRepository : Repository<Recipe>
    {
        public RecipeRepository(CookbookDbContext dbContext) : base(dbContext)
        {
        }

        public Recipe GetDetails(long id)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.Source)
                .Include(r => r.Steps)
                    .ThenInclude(step => step.IngredientDetails)
                        .ThenInclude(details => details.Ingredient)
                .Include(r => r.IngredientDetails)
                    .ThenInclude(details => details.Ingredient)
                .AsSplitQuery()
                .FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                throw new Exception($"Рецепт по ID {id} не найден.");

            return recipe;
        }
    }
}
