using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class EditIngredientCommandHandler : ICommand<EditIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditIngredientCommand command)
        {
            var oldIngredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == command.IngredientId);
            if (oldIngredient == null)
                throw new ArgumentException($"Ингредиента с ID {command.IngredientId} не существует.", nameof(command));

            oldIngredient.Name = command.NewName;
            _dbContext.SaveChanges();
        }
    }
}
