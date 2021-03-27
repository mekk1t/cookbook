using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class DeleteCategoryCommandHandler : ICommand<DeleteCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeleteCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(DeleteCategoryCommand command)
        {
            var category = _dbContext.Categories.FirstOrDefault(r => r.Name == command.Name);

            if (category == null)
                return;

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
    }
}
