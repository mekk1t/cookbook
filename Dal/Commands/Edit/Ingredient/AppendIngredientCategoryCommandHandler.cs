using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Ingredient
{
    public class AppendExistingCategoryToIngredientCommandHandler : ICommand<AppendExistingCategoryToIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendExistingCategoryToIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendExistingCategoryToIngredientCommand command)
        {
            var ingredient = _dbContext.Ingredients
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Id == command.IngredientId);
            if (ingredient == null)
                throw new ArgumentException(null, nameof(command));

            var category = _dbContext.Categories
                .AsNoTracking()
                .First(c => c.Name == command.CategoryName);

            ingredient.Categories.Add(new DbCategory(category.Id, category.Name));
            _dbContext.SaveChanges();
        }
    }
}
