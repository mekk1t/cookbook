using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public sealed class RecipeIngredientEditorTests
    {
        private readonly DbFixture _fixture;

        public RecipeIngredientEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Editor_appends_a_new_ingredient()
        {
            using var dbContext = _fixture.DbContext;
            var sut =
                new AppendIngredientToRecipeDecorator(
                    new AppendIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });

            sut.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new IngredientParameters(1.10M, Measures.ml)));

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(1);
        }

        [Fact]
        public void Editor_removes_an_ingredient()
        {
            using var dbContext = _fixture.DbContext;
            var appendIngredient =
                new AppendIngredientToRecipeDecorator(
                    new AppendIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new IngredientParameters(1.10M, Measures.ml)));
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new IngredientParameters(1.10M, Measures.ml)));
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Чтозачерт"),
                new IngredientParameters(1.10M, Measures.ml)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.Select(link => link.DbIngredientId).First();
            var sut =
                new RemoveIngredientFromRecipeDecorator(
                    new RemoveRecipeIngredientCommandHandler(dbContext),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)));

            sut.Execute(new RemoveIngredientFromRecipeCommand(recipeId, ingredientId));

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(2);
        }

        [Fact]
        public void Editor_replaces_an_ingredient()
        {
            using var dbContext = _fixture.DbContext;
            var appendIngredient =
                new AppendIngredientToRecipeDecorator(
                    new AppendIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));
            var recipeId = Guid.NewGuid();
            var newIngredientId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId, newIngredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new IngredientParameters(1.10M, Measures.ml)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.Select(link => link.DbIngredientId).First();
            var sut =
                new ReplaceIngredientInRecipeDecorator(
                    new ReplaceRecipeIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)));

            sut.Execute(
                new ReplaceRecipeIngredientCommand(
                    new Ingredient(
                        ingredientId,
                        "Хэйлялясан"),
                    new Ingredient(
                        newIngredientId,
                        newIngredientId.ToString()),
                    recipeId));

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(1);
            result.RecipeIngredientLink.First().DbIngredient.Name.Should().Be(newIngredientId.ToString());
        }

        [Fact]
        public void Editor_replaces_a_bunch_of_ingredients()
        {
            using var dbContext = _fixture.DbContext;
            var appendIngredient =
                new AppendIngredientToRecipeDecorator(
                    new AppendIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));
            var recipeId = Guid.NewGuid();
            var newIngredientId = Guid.NewGuid();
            var newIngredientId2 = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId, newIngredientId.ToString()));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(newIngredientId2, newIngredientId2.ToString()));
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new IngredientParameters(1.10M, Measures.ml)));
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Ахалай махалай"),
                new IngredientParameters(1.10M, Measures.ml)));
            var sut =
                new ReplaceIngredientsListInRecipeDecorator(
                    new ReplaceRecipeIngredientsListCommandHandler(dbContext),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));


           sut.Execute(new ReplaceRecipeIngredientsListCommand(
                new[]
                {
                    new Ingredient(newIngredientId, newIngredientId.ToString()),
                    new Ingredient(newIngredientId2, newIngredientId2.ToString())
                },
                recipeId));

            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Should().HaveCount(2);
            result.RecipeIngredientLink.Select(link => link.DbIngredient.Name)
                .Should()
                .OnlyContain(name => name == newIngredientId.ToString() || name == newIngredientId2.ToString());
        }

        [Fact]
        public void Editor_edits_ingredients_description()
        {
            using var dbContext = _fixture.DbContext;
            var appendIngredient =
                new AppendIngredientToRecipeDecorator(
                    new AppendIngredientCommandHandler(dbContext),
                    new RecipeChecker(
                        new GetRecipeQueryHandler(dbContext)),
                    new IngredientChecker(
                        new GetIngredientQueryHandler(dbContext)),
                    new CreateIngredientDecorator(
                        new CreateIngredientCommandHandler(dbContext),
                        new IngredientChecker(
                            new GetIngredientQueryHandler(dbContext)),
                        new CreateCategoryDecorator(
                            new CreateCategoryCommandHandler(dbContext),
                            new CategoryChecker(
                                new GetCategoryQueryHandler(dbContext)))));
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            appendIngredient.Execute(new AppendIngredientToRecipeCommand(
                recipeId,
                new Ingredient(Guid.NewGuid(), "Хэйлялясан"),
                new IngredientParameters(1.10M, Measures.ml)));
            var ingredientId = _fixture.FindRecipe(recipeId).RecipeIngredientLink.First().DbIngredientId;
            var sut =
                new EditRecipeIngredientDescriptionDecorator(
                    new EditRecipeIngredientDescriptionCommandHandler(dbContext));

            sut.Execute(new EditRecipeIngredientDescriptionCommand(recipeId, ingredientId, amount: 228M, measure: Measures.pc));

            var result = _fixture.FindRecipe(recipeId).RecipeIngredientLink.First();
            result.IngredientsAmount.Should().Be(228M);
            result.IngredientMeasure.Should().Be(Measures.pc);
        }
    }
}
