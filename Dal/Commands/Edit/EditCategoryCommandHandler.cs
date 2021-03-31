using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class EditCategoryCommandHandler : ICommand<EditCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditCategoryCommand command)
        {
            var oldCategory = _dbContext.Categories.FirstOrDefault(r => r.Id == command.Id);
            if (oldCategory == null)
                throw new ArgumentException($"Категории с ID {command.Id} не существует.");

            oldCategory.Name = command.NewName;
            _dbContext.SaveChanges();
        }
    }
}
