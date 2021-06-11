using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Ingredient
{
    public class RemoveIngredientCategoryCommandHandler : ICommand<RemoveIngredientCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public RemoveIngredientCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(RemoveIngredientCategoryCommand command)
        {
            var category = _dbContext.Categories
                .FirstOrDefault(c => c.Id == command.CategoryId);
            if (category == null)
                throw new Exception($"Не удалось найти категорию с ID {command.CategoryId}");

            var ingredient = _dbContext.Ingredients
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Id == command.IngredientId);
            if (ingredient == null)
                throw new ArgumentException($"Не удалось найти ингредиент с ID {command.IngredientId}");

            ingredient.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
    }
}
