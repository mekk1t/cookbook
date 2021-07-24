using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.Cookbook.Tests.DatabaseTests
{
    public class CategoryCrudTests
    {
        private readonly CategoryCrud _sut;

        public CategoryCrudTests()
        {
            _sut = new CategoryCrud()
        }

        [Fact]
        public void Нельзя_создать_категорию_с_пустым_именем()
        {
            var newCategory = new Category
            {
                Type = CategoryType.Ingredient,
                Name = null
            };

            var result =
        }
    }
}
