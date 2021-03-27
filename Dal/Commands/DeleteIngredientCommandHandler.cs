using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class DeleteIngredientCommandHandler : ICommand<DeleteIngredientCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteIngredientCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(DeleteIngredientCommand command)
        {
            var ingredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == command.IngredientId);
            if (ingredient == null)
            {
                return;
            }

            _dbContext.Ingredients.Remove(ingredient);
            _dbContext.SaveChanges();
        }
    }
}
