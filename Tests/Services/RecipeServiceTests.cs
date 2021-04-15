using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Services
{
    [Collection("Db")]
    public sealed class RecipeServiceTests : IDisposable
    {
        private readonly CreateRecipeDecorator _sut;
        private readonly DbFixture _fixture;
        private readonly List<AppDbContext> _dbContexts;

        public RecipeServiceTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContexts = new List<AppDbContext>();
            var dbContext = _fixture.DbContext;
            _dbContexts.Add(dbContext);
            var categoryService = new CreateCategoryDecorator(
                new CreateCategoryCommandHandler(dbContext),
                new CategoryChecker(
                    new GetCategoryQueryHandler(dbContext)));
            var ingredientService = new CreateIngredientDecorator(
                new CreateIngredientCommandHandler(dbContext),
                new IngredientChecker(
                    new GetIngredientQueryHandler(dbContext)),
                categoryService);
            _sut = new CreateRecipeDecorator(
                new CreateRecipeCommandHandler(dbContext),
                categoryService,
                ingredientService);
        }

        [Fact]
        public void New_recipe_creates_new_categories_and_ingredients()
        {
            var recipeId = Guid.NewGuid();

            Action act = () => _sut.Execute(new CreateRecipeCommand(
                recipeId,
                "Тестовый",
                new[] { "Ахалай махалай", "Букабяка" },
                new List<RecipeIngredientDetails>
                {
                    new RecipeIngredientDetails("Какао", Measures.Gramms, 20, "Сыпь не жалей!"),
                    new RecipeIngredientDetails("Шоколад", Measures.Gramms, 15, "Горький. > 65% какао.")
                },
                new List<RecipeStep>
                {
                    new RecipeStep(Guid.NewGuid())
                    {
                        Index = 1,
                        Description = "Делай давай",
                        IngredientsDetails =
                        {
                            new StepIngredientDetails
                            {
                                IngredientName = "Какао",
                                Amount = 20,
                                Measure = Measures.Gramms
                            }
                        }
                    }
                },
                "У этого рецепта две новые категории, два новых ингредиента и один шаг с одним из этих ингредиентов"
                ));

            act.Should().NotThrow();
            var result = _dbContexts.First().Recipes
                .AsNoTracking()
                .Include(r => r.RecipeCategoriesLink)
                .Include(r => r.RecipeIngredientLink)
                .Include(r => r.Steps).ThenInclude(s => s.StepIngredientsLink)
                .First(r => r.Id == recipeId);
            result.RecipeCategoriesLink.Should().HaveCount(2);
            result.RecipeIngredientLink.Should().HaveCount(2);
        }

        [Fact]
        public void New_recipe_is_created_with_existing_ingredients_and_categories()
        {
            var recipeId = Guid.NewGuid();
            var categoryName = Guid.NewGuid().ToString();
            var ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedCategory(new Category(Guid.NewGuid(), categoryName));
            _fixture.SeedCategory(new Category(Guid.NewGuid(), categoryName + "1"));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.NewGuid(), ingredientName));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.NewGuid(), ingredientName + "1"));

            Action act = () => _sut.Execute(new CreateRecipeCommand(
                recipeId,
                "Тестовый",
                new[] { categoryName, categoryName + "1" },
                new List<RecipeIngredientDetails>
                {
                    new RecipeIngredientDetails(ingredientName, Measures.Milliliters, 14, "Сыпь не жалей!"),
                    new RecipeIngredientDetails(ingredientName + "1", Measures.Gramms, 15, "Горький. > 65% какао.")
                },
                new List<RecipeStep>
                {
                    new RecipeStep(Guid.NewGuid())
                    {
                        Index = 1,
                        Description = "Делай давай",
                        IngredientsDetails =
                        {
                            new StepIngredientDetails
                            {
                                IngredientName = ingredientName,
                                Amount = 20,
                                Measure = Measures.Gramms
                            }
                        }
                    }
                }));

            act.Should().NotThrow();
            var result = _dbContexts.First().Recipes
                .AsNoTracking()
                .Include(r => r.RecipeCategoriesLink).ThenInclude(rc => rc.DbCategory)
                .Include(r => r.RecipeIngredientLink).ThenInclude(ri => ri.DbIngredient)
                .Include(r => r.Steps).ThenInclude(s => s.StepIngredientsLink)
                .First(r => r.Id == recipeId);
            result.RecipeCategoriesLink.Should().HaveCount(2);
            result.RecipeIngredientLink.Should().HaveCount(2);
            result.RecipeCategoriesLink.Select(link => link.DbCategory).Select(c => c.Name).Should().Contain(categoryName);
            result.RecipeIngredientLink.Select(link => link.DbIngredient).Select(i => i.Name).Should().Contain(ingredientName);
        }

        [Fact]
        public void Deleting_recipe_deletes_its_stages_but_not_anything_else()
        {
            using var dbContext = _fixture.DbContext;
            var recipeId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var ingredientId = Guid.NewGuid();
            var categoryName1 = Guid.NewGuid().ToString();
            var ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedCategory(new Category(categoryId, categoryName1));
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientName));
            var seedRecipe = new DbRecipe
            {
                Id = recipeId,
                Description = "Description",
                Title = "Title",
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Description = "",
                        Index = 1,
                        Image = "",
                        StepIngredientsLink = new List<DbRecipeStepIngredient>
                        {
                            new DbRecipeStepIngredient
                            {
                                DbRecipeStepId = stepId,
                                DbIngredientId = ingredientId
                            }
                        }
                    }
                },
                RecipeCategoriesLink = new List<DbRecipeCategory>
                {
                    new DbRecipeCategory
                    {
                        DbRecipeId = recipeId,
                        DbCategoryId = categoryId
                    }
                },
                RecipeIngredientLink = new List<DbRecipeIngredient>
                {
                    new DbRecipeIngredient
                    {
                        DbRecipeId = recipeId,
                        DbIngredientId = ingredientId,
                        IngredientMeasure = Measures.None,
                        IngredientxAmount = 1
                    }
                }
            };
            _fixture.SeedRecipe(seedRecipe);
            var sut = new DeleteRecipeCommandHandler(dbContext);

            Action act = () => sut.Execute(new DeleteRecipeCommand(recipeId));

            act.Should().NotThrow();
            var deletedRecipe = dbContext.Recipes.FirstOrDefault(r => r.Id == recipeId).Should().BeNull();
            var deletedStep = dbContext.Find(typeof(DbRecipeStep), stepId).Should().BeNull();
            var oldIngredient = dbContext.Ingredients.First(r => r.Id == ingredientId).Should().NotBeNull();
            var oldCategory = dbContext.Categories.First(c => c.Id == categoryId).Should().NotBeNull();
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
