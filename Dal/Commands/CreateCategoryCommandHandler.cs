using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class CreateCategoryCommandHandler : ICommand<CreateCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreateCategoryCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(CreateCategoryCommand command)
        {
            var newCategory = new Category(Guid.NewGuid(), command.Name);
            _dbContext.Categories.Add(newCategory);
            _dbContext.SaveChanges();
        }
    }
}
