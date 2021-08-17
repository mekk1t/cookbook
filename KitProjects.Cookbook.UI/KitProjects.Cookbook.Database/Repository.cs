using KitProjects.Cookbook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.Cookbook.Database
{
    public class Repository<T> where T : Entity
    {
        protected readonly CookbookDbContext _dbContext;

        public Repository(CookbookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Save(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }

        public List<T> GetList(long lastId = 0, int limit = 10)
        {
            return _dbContext.Set<T>()
                .AsNoTracking()
                .Where(r => r.Id > lastId)
                .OrderBy(e => e.Id)
                .Take(limit)
                .ToList();
        }

        public T GetOrDefault(long id) => _dbContext.Set<T>().AsNoTracking().FirstOrDefault(entity => entity.Id == id);
    }
}
