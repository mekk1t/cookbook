using KitProjects.Cookbook.Core.Models;
using System.Collections.Generic;

namespace KitProjects.Cookbook.Core.Abstractions
{
    public interface IRepository<TEntity, TFilter>
        where TEntity : Entity
        where TFilter : PaginationFilter
    {
        List<TEntity> GetList(TFilter filter = default);
    }
}
