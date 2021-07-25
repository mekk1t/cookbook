using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.Cookbook.Database.Crud
{
    public class CategoryCrud : ICrud<Category, long>, IRepository<Category, PaginationFilter>
    {
        private readonly CookbookDbContext _dbContext;

        public CategoryCrud(CookbookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category Create(Category entity)
        {
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();

            return new Category(entity);
        }

        public void Delete(Category entity)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == entity.Id);
            category.ThrowIfEntityIsNull(entity.Id);

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }

        public Category Read(long key)
        {
            var category = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.Id == key);
            category.ThrowIfEntityIsNull(key);
            return category;
        }

        public Category Update(Category entity)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == entity.Id);
            category.ThrowIfEntityIsNull(entity.Id);

            category.Name = entity.Name;

            _dbContext.SaveChanges();
            return new Category(entity);
        }

        public List<Category> GetList(PaginationFilter filter = null)
        {
            if (filter == null)
                filter = new PaginationFilter();

            return _dbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .Where(c => c.Id >= filter.LastId)
                .Take(filter.Limit)
                .ToList();
        }
    }
}
