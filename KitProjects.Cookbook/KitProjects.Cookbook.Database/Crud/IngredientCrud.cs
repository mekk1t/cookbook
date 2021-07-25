using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.Cookbook.Database.Crud
{
    public class IngredientCrud : ICrud<Ingredient, long>, IRepository<Ingredient, PaginationFilter>
    {
        private readonly CookbookDbContext _dbContext;

        public IngredientCrud(CookbookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Ingredient Create(Ingredient entity)
        {
            var dbIngredient = new Ingredient(entity);
            LoadExistingCategories(dbIngredient);

            _dbContext.Ingredients.Add(dbIngredient);
            _dbContext.SaveChanges();

            return new Ingredient(dbIngredient);
        }

        private void LoadExistingCategories(Ingredient entity)
        {
            if (entity.Categories.Any(c => c.Id != default))
            {
                for (int i = 0; i < entity.Categories.Count; i++)
                {
                    var currentCategory = entity.Categories[i];
                    if (currentCategory.Id == default)
                        continue;
                    else
                    {
                        entity.Categories[i] = _dbContext.Categories.First(c => c.Id == currentCategory.Id);
                    }
                }
            }
        }

        public void Delete(Ingredient entity)
        {
            var dbIngredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == entity.Id);
            dbIngredient.ThrowIfEntityIsNull(entity.Id);

            _dbContext.Ingredients.Remove(dbIngredient);
            _dbContext.SaveChanges();
        }

        public List<Ingredient> GetList(PaginationFilter filter = null)
        {
            if (filter == null)
                filter = new();

            return _dbContext.Ingredients
                .AsNoTracking()
                .Include(i => i.Categories)
                .Where(i => i.Id >= filter.LastId)
                .OrderBy(i => i.Id)
                .Take(filter.Limit)
                .ToList();
        }

        public Ingredient Read(long key)
        {
            var dbIngredient = _dbContext.Ingredients
                .AsNoTracking()
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Id == key);
            dbIngredient.ThrowIfEntityIsNull(key);
            return dbIngredient;
        }

        public Ingredient Update(Ingredient entity)
        {
            var dbIngredient = _dbContext.Ingredients.FirstOrDefault(i => i.Id == entity.Id);
            dbIngredient.ThrowIfEntityIsNull(entity.Id);

            dbIngredient.Name = entity.Name;
            _dbContext.SaveChanges();
            return new Ingredient(dbIngredient);
        }
    }
}
