using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.MasterChef.Tests.Services
{
    [Collection("Db")]
    public sealed class RecipeServiceTests : IDisposable
    {
        private readonly RecipeService _sut;
        private readonly DbFixture _fixture;
        private readonly List<AppDbContext> _dbContexts;

        public RecipeServiceTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContexts = new List<AppDbContext>();
            var dbContext = _fixture.DbContext;
            _dbContexts.Add(dbContext);
            var categoryService = new CategoryService(
                new CreateCategoryCommandHandler(dbContext),
                new GetCategoriesQueryHandler(dbContext),
                new DeleteCategoryCommandHandler(dbContext),
                new EditCategoryCommandHandler(dbContext));
            var ingredientService = new IngredientService(
                new CreateIngredientCommandHandler(dbContext),
                new GetIngredientsQueryHandler(dbContext),
                categoryService,
                new EditIngredientCommandHandler(dbContext),
                new DeleteIngredientCommandHandler(dbContext));
            _sut = new RecipeService(new CreateRecipeCommandHandler(dbContext), categoryService, ingredientService, new GetRecipesQueryHandler(dbContext));
        }

        [Fact]
        public void New_recipe_creates_new_categories_and_ingredients()
        {
            var recipeId = Guid.NewGuid();

            Action act = () => _sut.CreateRecipe(new CreateRecipeCommand(
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
                        IngredientsDetails = new List<StepIngredientDetails>
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
            _fixture.SeedIngredient(new Ingredient(Guid.NewGuid(), ingredientName));
            _fixture.SeedIngredient(new Ingredient(Guid.NewGuid(), ingredientName + "1"));

            Action act = () => _sut.CreateRecipe(new CreateRecipeCommand(
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
                        IngredientsDetails = new List<StepIngredientDetails>
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


        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
