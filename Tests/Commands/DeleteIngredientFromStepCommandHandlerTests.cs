using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Delete;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public class DeleteIngredientFromStepCommandHandlerTests
    {
        private readonly DbFixture _fixture;

        public DeleteIngredientFromStepCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Step_has_one_ingredient_less_after_deleting()
        {
            using var dbContext = _fixture.DbContext;
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        StepIngredientsLink = new List<DbRecipeStepIngredient>
                        {
                            new DbRecipeStepIngredient
                            {
                                DbRecipeStepId = stepId,
                                DbIngredientId = ingredientId
                            }
                        }
                    }
                }
            });
            var sut = new DeleteStepIngredientCommandHandler(dbContext);

            sut.Execute(new DeleteIngredientFromStepCommand(new RecipeStepIds(recipeId, stepId), ingredientId));

            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First().StepIngredientsLink.Should().BeEmpty();
        }
    }
}
