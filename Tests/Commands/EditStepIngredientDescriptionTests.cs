using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public class EditStepIngredientDescriptionTests
    {
        private readonly DbFixture _fixture;

        public EditStepIngredientDescriptionTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Editing_step_ingredient_information()
        {
            using var dbContext = _fixture.DbContext;
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var ingredientId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Kernel.Models.Ingredient(ingredientId, ingredientId.ToString()));
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
            var sut = new EditStepIngredientDescriptionCommandHandler(dbContext);

            sut.Execute(
                new EditStepIngredientDescriptionCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    ingredientId,
                    12.48M,
                    Measures.Gramms));

            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First().StepIngredientsLink.First().Amount.Should().Be(12.5M);
        }
    }
}
