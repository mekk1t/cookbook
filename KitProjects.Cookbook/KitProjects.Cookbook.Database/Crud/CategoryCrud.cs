using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KitProjects.Cookbook.Database.Crud
{
    public class CategoryCrud : ICrud<Category, long>
    {
        private readonly CookbookDbContext _dbContext;

        public CategoryCrud(CookbookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Category Create(Category entity)
        {
            LookForExistingIngredients(entity);

            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(Category entity)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == entity.Id);
            category.ThrowIfNull();

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }

        public Category Read(long key)
        {
            var category = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.Id == key);
            category.ThrowIfNull();
            return category;
        }

        public Category Update(Category entity)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == entity.Id);
            category.ThrowIfNull();

            UpdateCategory(category, entity);
            LookForExistingIngredients(category);

            _dbContext.SaveChanges();
            return new Category(category.Id)
            {
                Name = category.Name,
                Type = category.Type,
                Ingredients = category.Ingredients.Select(i => new Ingredient(i.Id) { Name = i.Name }).ToList()
            };
        }

        private void LookForExistingIngredients(Category entity)
        {
            if (entity.Ingredients?.Any(i => i.Id != default) ?? false)
            {
                for (int i = 0; i < entity.Ingredients.Count; i++)
                {
                    var ingredient = entity.Ingredients[i];
                    if (ingredient.Id == default)
                        continue;

                    entity.Ingredients[i] = _dbContext.Ingredients.First(i => i.Id == ingredient.Id);
                }
            }
        }

        private static void UpdateCategory(Category oldCategory, Category newCategory)
        {
            oldCategory.Type = newCategory.Type;
            oldCategory.Name = newCategory.Name;
            oldCategory.Ingredients = newCategory.Ingredients;
        }
    }
}
