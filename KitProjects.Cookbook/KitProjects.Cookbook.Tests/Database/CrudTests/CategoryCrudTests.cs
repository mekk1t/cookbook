using FluentAssertions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Database.Crud;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace KitProjects.Cookbook.Tests.Database.CrudTests
{
    [Collection(nameof(DbFixture))]
    public class CategoryCrudTests : DatabaseTests
    {
        private readonly CategoryCrud _sut;

        public CategoryCrudTests(DbFixture dbFixture) : base(dbFixture)
        {
            _sut = new CategoryCrud(_dbContext);
        }

        [Fact]
        public void Нельзя_создать_категорию_с_пустым_именем()
        {
            var newCategory = new Category
            {
                Type = CategoryType.Ingredient,
                Name = null
            };

            Action act = () => _sut.Create(newCategory);

            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void Успешное_создание_валидной_категории_без_ингредиентов()
        {
            var newCategory = new Category
            {
                Type = CategoryType.Recipe,
                Name = "Heisenberg"
            };

            var result = _sut.Create(newCategory);

            result.Name.Should().Be("Heisenberg");
        }

        [Fact]
        public void Круд_удаляет_категорию_по_ID()
        {
            var existingCategory = SeedCategory("МАФАКА");

            Action act = () => _sut.Delete(existingCategory);

            act.Should().NotThrow();
        }

        [Fact]
        public void Нельзя_удалить_несуществующую_категорию()
        {
            var newCategory = new Category(long.MaxValue);

            Action act = () => _sut.Delete(newCategory);

            act.Should().Throw<EntityNotFoundException>();
        }

        [Fact]
        public void Можно_отредактировать_тип_и_имя_категории()
        {
            var category = SeedCategory(Guid.NewGuid().ToString());
            var editedCategory = new Category(category.Id)
            {
                Name = "Новое имя",
                Type = CategoryType.Ingredient
            };

            var result = _sut.Update(editedCategory);

            result.Type.Should().Be(CategoryType.Ingredient);
        }

        private Ingredient SeedIngredient(string name)
        {
            var entity = new Ingredient { Name = name };
            _dbContext.Ingredients.Add(entity);
            _dbContext.SaveChanges();
            return new Ingredient(entity);
        }

        private Category SeedCategory(string name) =>
            _sut.Create(new Category { Name = name });

        private static Category CreateCategoryWithIngredients(params string[] ingredientNames)
        {
            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
                Type = CategoryType.Recipe
            };

            category.Ingredients.AddRange(ingredientNames.Select(name => new Ingredient { Name = name }));

            return category;
        }
    }
}
