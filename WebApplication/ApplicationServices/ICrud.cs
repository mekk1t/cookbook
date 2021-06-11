using KitProjects.MasterChef.Kernel;
using System;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public interface ICrud<TModel> where TModel : Entity
    {
        TModel Details(Guid id);
        void Delete(Guid id);
    }
}
