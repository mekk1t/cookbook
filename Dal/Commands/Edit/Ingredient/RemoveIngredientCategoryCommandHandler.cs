using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            var category = _dbContext.Categories.First(c => c.Name == command.CategoryName);
            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == command.IngredientId);
            if (ingredient == null)
                throw new ArgumentException(null, nameof(command));

            ingredient.Categories.Remove(new DbCategory(category.Id, category.Name));
            _dbContext.SaveChanges();
        }
    }
}
