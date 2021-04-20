using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class AppendCategoryCommandHandler : ICommand<AppendCategoryToRecipeCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendCategoryToRecipeCommand command)
        {
            var category = _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.Name == command.CategoryName);
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeCategoriesLink)
                .First(r => r.Id == command.RecipeId);
            recipe.RecipeCategoriesLink.Add(new DbRecipeCategory
            {
                DbRecipeId = recipe.Id,
                DbCategoryId = category.Id
            });
            _dbContext.SaveChanges();
        }
    }
}
