using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Steps;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public class ReplaceStepCommandHandlerTests
    {
        private readonly DbFixture _fixture;

        public ReplaceStepCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Replacing_step_does_not_create_new_ingredients()
        {

        }

        [Fact]
        public void New_step_has_the_same_index_as_old_step()
        {
            using var dbContext = _fixture.DbContext;
            var sut = new ReplaceStepCommandHandler(dbContext);
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.NewGuid(), "Старый"));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new Collection<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Index = 1
                    }
                }
            });

            Action act = () => sut.Execute(new ReplaceStepCommand(
                recipeId, stepId, "Блабла", "124912019248",
                new List<StepIngredientDetails>
                {
                    new StepIngredientDetails
                    {
                        IngredientName = "Новый",
                        Amount = 1,
                        Measure = Measures.Milliliters
                    },
                    new StepIngredientDetails
                    {
                        IngredientName = "Старый",
                        Amount = 12,
                        Measure = Measures.Pieces
                    }
                }));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First().StepIngredientsLink.Should().HaveCount(1);
            result.Steps.First().Index.Should().Be(1);
        }

        [Fact]
        public void New_step_with_nonexistent_ingredients_has_no_ingredients()
        {
            using var dbContext = _fixture.DbContext;
            var sut = new ReplaceStepCommandHandler(dbContext);
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new Collection<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Index = 1
                    }
                }
            });

            Action act = () => sut.Execute(new ReplaceStepCommand(
                recipeId, stepId, "Блабла", "124912019248",
                new List<StepIngredientDetails>
                {
                    new StepIngredientDetails
                    {
                        IngredientName = "Новый",
                        Amount = 1,
                        Measure = Measures.Milliliters
                    },
                    new StepIngredientDetails
                    {
                        IngredientName = "Старый",
                        Amount = 12,
                        Measure = Measures.Pieces
                    }
                }));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First().StepIngredientsLink.Should().BeEmpty();
            result.Steps.First().Index.Should().Be(1);
        }
    }
}
