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
            _sut = new RecipeService(new CreateRecipeCommandHandler(dbContext), categoryService, ingredientService);
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



        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
