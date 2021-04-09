using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Ingredient
{
    public class AppendIngredientCategoryCommandHandler : ICommand<AppendIngredientCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public AppendIngredientCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(AppendIngredientCategoryCommand command)
        {
            var ingredient = _dbContext.Ingredients
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Id == command.IngredientId);
            if (ingredient == null)
                throw new ArgumentException(null, nameof(command));

            var category = _dbContext.Categories.First(c => c.Name == command.CategoryName);

            ingredient.Categories.Add(new DbCategory(category.Id, category.Name));
            _dbContext.SaveChanges();
        }
    }
}
