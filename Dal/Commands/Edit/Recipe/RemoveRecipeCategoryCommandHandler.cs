using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class RemoveRecipeCategoryCommandHandler : ICommand<RemoveRecipeCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveRecipeCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(RemoveRecipeCategoryCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeCategoriesLink).ThenInclude(link => link.DbCategory)
                .First(r => r.Id == command.RecipeId);
            if (!recipe.RecipeCategoriesLink.Select(link => link.DbCategory.Name).Contains(command.CategoryName))
                return;

            recipe.RecipeCategoriesLink.Remove(recipe.RecipeCategoriesLink.First(rc => rc.DbCategory.Name == command.CategoryName));
            _dbContext.SaveChanges();
        }
    }
}
