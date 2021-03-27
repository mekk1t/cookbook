using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class CreateCategoryCommandHandler : ICommand<CreateCategoryCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreateCategoryCommandHandler()
        {

        }

        public void Execute(CreateCategoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
