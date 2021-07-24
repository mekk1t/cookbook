using FluentAssertions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database.Crud;
using System;
using Xunit;

namespace KitProjects.Cookbook.Tests.Database
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

            act.Should().Throw<Exception>();
        }
    }
}
