using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Abstractions
{
    public interface IRepository<TModel>
    {
        void Create(TModel model);
        IEnumerable<TModel> Read(Func<TModel, bool> filterLogic);
        void Update(TModel model);
        void Delete(TModel model);
    }
}
