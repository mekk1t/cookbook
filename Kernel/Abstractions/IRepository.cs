using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Abstractions
{
    public interface IRepository<TModel, TId>
    {
        void Create(TModel model);
        IEnumerable<TModel> Read(ListQueryFilter filter);
        TModel Find(TId id);
        void Update(TModel model);
        void Delete(TModel model);
    }
}
