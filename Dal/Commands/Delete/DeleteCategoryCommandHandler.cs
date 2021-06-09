using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
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
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (command.Id == default)
                throw new ArgumentException("ID категории не может быть значением по умолчанию.");

            var category = _dbContext.Categories.FirstOrDefault(r => r.Id == command.Id);

            if (category == null)
                throw new ArgumentException($"Не удалось найти категорию, которую пытались удалить. Её ID: {command.Id}");

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
    }
}
