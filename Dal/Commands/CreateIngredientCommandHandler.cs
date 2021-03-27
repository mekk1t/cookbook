using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class CreateIngredientCommandHandler : ICommand<CreateIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreateIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(CreateIngredientCommand command)
        {
            var dbCategories = _dbContext.Categories
                .AsNoTracking()
                .Where(c => command.Categories.Contains(c.Name))
                .ToList();
            _dbContext.Ingredients.Add(new DbIngredient(Guid.NewGuid(), command.Name, dbCategories));
            _dbContext.SaveChanges();
        }
    }
}
