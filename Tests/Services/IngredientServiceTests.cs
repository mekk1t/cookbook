using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Services
{
    [Collection("Db")]
    public class IngredientServiceTests
    {
        private readonly DbFixture _fixture;
        private readonly List<DbContext> _dbContexts;
        private readonly IngredientService _sut;

        public IngredientServiceTests(DbFixture fixture)
        {
            _dbContexts = new List<DbContext>();
            _fixture = fixture;
            var dbContext = _fixture.DbContext;
            _dbContexts.Add(dbContext);
            _sut = new IngredientService(
                new CreateIngredientCommandHandler(dbContext),
                new GetIngredientsQueryHandler(dbContext),
                new CreateCategoryDecorator(
                    new CreateCategoryCommandHandler(dbContext),
                    new GetCategoriesQueryHandler(dbContext)),
                new GetCategoriesQueryHandler(dbContext));
        }

        [Fact]
        public void Can_create_an_ingredient_without_categories()
        {
            var ingredientName = "Ингредиент";

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new List<string>()));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Categories.Should().BeEmpty();
            result.Name.Should().Be(ingredientName);
        }

        [Fact]
        public void Can_create_an_ingredient_with_nonexistent_categories()
        {
            using var dbContext = _fixture.DbContext;
            var ingredientName = Guid.NewGuid().ToString();
            var newCategories = new List<string>()
            {
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()
            };

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, newCategories));

            act.Should().NotThrow();
            dbContext.Categories.AsNoTracking().Select(c => c.Name).Should().Contain(newCategories);
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Name.Should().Be(ingredientName);
            result.Categories.Select(c => c.Name).Should().Contain(newCategories);
        }

        [Fact]
        public void Can_create_an_ingredient_with_existing_categories()
        {
            string ingredientName = "Новый ингредиент";
            _fixture.SeedCategory(new Category(Guid.NewGuid(), "1"));
            _fixture.SeedCategory(new Category(Guid.NewGuid(), "2"));

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new[] { "1", "2" }));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().NotBeNull();
            result.Name.Should().Be(ingredientName);
            result.Categories.Select(c => c.Name).Should().Contain(new[] { "1", "2" });
        }

        [Fact]
        public void Cant_create_a_duplicate_name_ingredient()
        {
            string ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.NewGuid(), ingredientName));

            Action act = () => _sut.CreateIngredient(new CreateIngredientCommand(ingredientName, new List<string>()));

            act.Should().NotThrow();
            using var dbContext = _fixture.DbContext;
            dbContext.Ingredients.Where(i => i.Name == ingredientName).Should().HaveCount(1);
        }

        [Fact]
        public void Edit_ingredient_name_jesus_im_bored()
        {
            var ingredientId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, "влтывдлмтыд"));
            using var dbContext = _fixture.DbContext;
            var sut = new EditIngredientCommandHandler(dbContext);

            Action act = () => sut.Execute(new EditIngredientCommand(ingredientId, "НовоеИмя"));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient("НовоеИмя");
            result.Should().NotBeNull();
        }

        [Fact]
        public void Delete_ingredient_by_id()
        {
            var ingredientId = Guid.NewGuid();
            var ingredientName = "414019гк30";
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientName));
            using var dbContext = _fixture.DbContext;
            var sut = new DeleteIngredientCommandHandler(dbContext);

            Action act = () => sut.Execute(new DeleteIngredientCommand(ingredientId));

            act.Should().NotThrow();
            var result = _fixture.FindIngredient(ingredientName);
            result.Should().BeNull();
        }

        [Fact]
        public void Ingredients_query_with_relationships_gets_all_related_categories()
        {
            _sut.CreateIngredient(new CreateIngredientCommand("Тестовый", new[] { "Абвгд", "Еёжз" }));
            var query = new GetIngredientsQuery(withRelationships: true);
            using var dbContext = _fixture.DbContext;
            var sut = new GetIngredientsQueryHandler(dbContext);

            var result = sut.Execute(query);

            result.First(r => r.Name == "Тестовый")
                .Categories.Select(c => c.Name).Should().Contain(new[] { "Абвгд", "Еёжз" });
        }

        [Fact]
        public void Ingredients_query_without_relationships_doesnt_have_related_categories()
        {
            _sut.CreateIngredient(new CreateIngredientCommand("Тестовый", new[] { "Абвгд", "Еёжз" }));
            var query = new GetIngredientsQuery(withRelationships: false);
            using var dbContext = _fixture.DbContext;
            var sut = new GetIngredientsQueryHandler(dbContext);

            var result = sut.Execute(query);

            result.First(r => r.Name == "Тестовый")
                .Categories.Select(c => c.Name).Should().NotContain(new[] { "Абвгд", "Еёжз" });
        }
    }
}
