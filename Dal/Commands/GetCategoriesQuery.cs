using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetCategoriesQuery : IQuery<IEnumerable<Category>>
    {
        public IEnumerable<Category> Execute()
        {
            throw new NotImplementedException();
        }
    }
}
