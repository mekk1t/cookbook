using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Moderators
{
    [Collection("Db")]
    public sealed class CategoryServiceTests : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly CreateCategoryDecorator _sut;
        private readonly CreateIngredientDecorator _ingredientService;
        private List<DbContext> _dbContexts;

        public CategoryServiceTests(DbFixture fixture)
        {
            _fixture = fixture;
            var dbContext = _fixture.DbContext;
            _dbContexts = new List<DbContext>
            {
                dbContext
            };
            _sut = new CreateCategoryDecorator(
                new CreateCategoryCommandHandler(dbContext),
                new CategoryChecker(
                    new GetCategoryQueryHandler(dbContext)));
            _ingredientService = new CreateIngredientDecorator(
                new CreateIngredientCommandHandler(dbContext),
                new IngredientChecker(
                    new GetIngredientQueryHandler(dbContext)),
                _sut);
        }

        [Fact]
        public void Category_moderator_creates_a_new_category()
        {
            Action act = () => _sut.Execute(new CreateCategoryCommand("Тест"));

            act.Should().NotThrow();
            using var _dbContext = _fixture.DbContext;
            var result = _dbContext.Categories.First(r => r.Name == "Тест");
            result.Should().NotBeNull();
            result.Name.Should().Be("Тест");
        }

        [Fact]
        public void Cant_create_a_new_category_with_existing_name()
        {
            var categoryName = "бавгд";
            _fixture.SeedCategory(new Category(Guid.NewGuid(), categoryName));
            using var dbContext = _fixture.DbContext;

            Action act = () => _sut.Execute(new CreateCategoryCommand(categoryName));

            act.Should().NotThrow();
            dbContext.Categories.Where(r => r.Name == categoryName).Should().HaveCount(1);
        }

        [Fact]
        public void Moderator_deletes_category_by_name()
        {
            var categoryId = Guid.NewGuid();
            _fixture.SeedCategory(new Category(categoryId, categoryId.ToString()));
            using var dbContext = _fixture.DbContext;
            var sut = new DeleteCategoryCommandHandler(dbContext);

            Action act = () => sut.Execute(new DeleteCategoryCommand(categoryId));

            act.Should().NotThrow();
            dbContext.Categories.AsNoTracking().Where(r => r.Id == categoryId).Should().BeEmpty();
        }

        [Fact]
        public void Moderator_does_throw_when_deleting_category_does_not_exist()
        {
            var randomId = Guid.NewGuid();
            using var dbContext = _fixture.DbContext;
            var sut = new DeleteCategoryCommandHandler(dbContext);

            Action act = () => sut.Execute(new DeleteCategoryCommand(randomId));

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Can_edit_category()
        {
            var categoryId = Guid.NewGuid();
            var oldName = "12345";
            var newName = "НовоеИмя";
            _fixture.SeedCategory(new Category(categoryId, oldName));
            using var dbContext = _fixture.DbContext;
            var sut = new EditCategoryCommandHandler(dbContext);

            Action act = () => sut.Execute(new EditCategoryCommand(categoryId, newName));

            act.Should().NotThrow();
            var category = _fixture.FindCategory(newName);
            category.Should().NotBeNull();
            category.Name.Should().Be(newName);
            dbContext.Categories.Where(c => c.Name == oldName).Should().BeEmpty();
        }

        [Fact]
        public void Category_query_with_relationships_gets_all_ingredients_related()
        {
            var ingredientName = "вжыьлдмывмлд";
            _ingredientService.Execute(new CreateIngredientCommand(ingredientName, new[] { "Категория1", "Категория2" }));
            var query = new GetCategoriesQuery(withRelationships: true);
            using var dbContext = _fixture.DbContext;
            var sut = new GetCategoriesQueryHandler(dbContext);

            var result = sut.Execute(query);

            var categoriesWithIngredients = result.Where(r => r.Name == "Категория1" || r.Name == "Категория2");
            categoriesWithIngredients.Select(c => c.Name).Should().Contain(new[] { "Категория1", "Категория2" });
        }

        [Fact]
        public void Category_query_without_relationships_doesnt_have_related_ingredients()
        {
            var ingredientName = "as;lvmasd;lvmsd;lvm";
            _ingredientService.Execute(new CreateIngredientCommand(ingredientName, new[] { "Категория1", "Категория2" }));
            var query = new GetCategoriesQuery(withRelationships: false);
            using var dbContext = _fixture.DbContext;
            var sut = new GetCategoriesQueryHandler(dbContext);

            var result = sut.Execute(query);

            result.ToList().ForEach(category => category.Ingredients.Should().BeNullOrEmpty());
        }

        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
