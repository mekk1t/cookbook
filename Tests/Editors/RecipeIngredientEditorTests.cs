using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public sealed class RecipeIngredientEditorTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly RecipeIngredientEditor _sut;

        public RecipeIngredientEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            _sut = new RecipeIngredientEditor(
                new SearchIngredientQueryHandler(_dbContext),
                new SearchRecipeQueryHandler(_dbContext),
                new AppendIngredientCommandHandler(_dbContext),
                new RemoveRecipeIngredientCommandHandler(_dbContext),
                new ReplaceRecipeIngredientsListCommandHandler(_dbContext),
                new ReplaceRecipeIngredientCommandHandler(_dbContext),
                new IngredientService(
                    new CreateIngredientCommandHandler(_dbContext),
                    new GetIngredientsQueryHandler(_dbContext),
                    new CategoryService(
                        new CreateCategoryCommandHandler(_dbContext),
                        new GetCategoriesQueryHandler(_dbContext),
                        new DeleteCategoryCommandHandler(_dbContext),
                        new EditCategoryCommandHandler(_dbContext)),
                    new EditIngredientCommandHandler(_dbContext),
                    new DeleteIngredientCommandHandler(_dbContext)),
                new EditRecipeIngredientDescriptionCommandHandler(_dbContext));
        }

        public void Dispose() => _dbContext.Dispose();

        [Fact]
        public void Editor_appends_a_new_ingredient()
        {
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });

            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(1);
        }

        [Fact]
        public void Editor_removes_an_ingredient()
        {
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Чтозачерт"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.Select(link => link.DbIngredientId).First();

            _sut.RemoveIngredient(recipeId, ingredientId);

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(2);
        }

        [Fact]
        public void Editor_replaces_an_ingredient()
        {
            var recipeId = Guid.NewGuid();
            var newIngredientId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId, newIngredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.Select(link => link.DbIngredientId).First();

            _sut.ReplaceIngredient(
                new Ingredient(ingredientId, "Хэйлялясан"),
                new Ingredient(newIngredientId, newIngredientId.ToString()),
                recipeId);

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(1);
            result.RecipeIngredientLink.First().DbIngredient.Name.Should().Be(newIngredientId.ToString());
        }

        [Fact]
        public void Editor_replaces_a_bunch_of_ingredients()
        {
            var recipeId = Guid.NewGuid();
            var newIngredientId = Guid.NewGuid();
            var newIngredientId2 = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId, newIngredientId.ToString()));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId2, newIngredientId2.ToString()));
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));

            _sut.ReplaceIngredientsList(
                new[]
                {
                    new Ingredient(newIngredientId, newIngredientId.ToString()),
                    new Ingredient(newIngredientId2, newIngredientId2.ToString())
                },
                recipeId);

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(2);
            result.RecipeIngredientLink.Select(link => link.DbIngredient.Name)
                .Should()
                .OnlyContain(name => name == newIngredientId.ToString() || name == newIngredientId2.ToString());
        }

        [Fact]
        public void Editor_edits_ingredients_description()
        {
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            _sut.AppendIngredient(new AppendRecipeIngredientCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new AppendIngredientParameters(1.10M, Measures.Milliliters)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.First().DbIngredientId;

            _sut.EditIngredientsDescription(
                new EditRecipeIngredientDescriptionCommand(recipeId, ingredientId, amount: 228M, measure: Measures.Pieces));

            var result = _fixture.FindRecipe(recipeId).RecipeIngredientLink.First();
            result.IngredientxAmount.Should().Be(228M);
            result.IngredientMeasure.Should().Be(Measures.Pieces);
        }
    }
}
