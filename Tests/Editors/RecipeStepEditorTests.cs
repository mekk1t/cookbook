using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Dal.Queries.Steps;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class RecipeStepEditorTests : IDisposable
    {
        private readonly List<DbContext> _dbContexts = new();
        private readonly RecipeStepEditor _sut;
        private readonly DbFixture _fixture;

        public RecipeStepEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
            var queryDbContext = _fixture.DbContext;
            var editDbContext = _fixture.DbContext;
            var swapDbContext = _fixture.DbContext;
            _sut = new RecipeStepEditor(
                new EditStepPictureCommandHandler(editDbContext),
                new EditStepDescriptionCommandHandler(editDbContext),
                new SearchStepQueryHandler(queryDbContext),
                new SwapStepsCommandHandler(swapDbContext),
                new SearchRecipeQueryHandler(queryDbContext),
                new AppendRecipeStepCommandHandler(editDbContext),
                new RemoveRecipeStepCommandHandler(editDbContext));

            _dbContexts.AddRange(new[] { queryDbContext, editDbContext, swapDbContext });
        }

        [Fact]
        public void Editor_edits_step_description()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Description = "Аллах акбар"
                    }
                }
            });
            var newDescription = "Что за черт?";

            Action act = () => _sut.ChangeDescription(stepId, newDescription);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First(step => step.Id == stepId).Description.Should().Be(newDescription);
        }

        [Fact]
        public void Editor_cant_edit_description_of_nonexistent_step()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });
            var newDescription = "Что за черт?";

            Action act = () => _sut.ChangeDescription(stepId, newDescription);

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_edits_picture_of_the_step()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Image = "SomeImage"
                    }
                }
            });
            var newImage = "Новое изображение";

            Action act = () => _sut.ChangePicture(stepId, newImage);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.First(step => step.Id == stepId).Image.Should().Be(newImage);
        }

        [Fact]
        public void Editor_cant_edit_picture_of_nonexistent_step()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });
            var newImage = "Новое изображение";

            Action act = () => _sut.ChangePicture(stepId, newImage);

            act.Should().Throw<Exception>();
        }

        [Fact]
        public void Editor_swaps_two_steps()
        {
            var recipeId = Guid.NewGuid();
            var firstStepId = Guid.NewGuid();
            var secondStepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = firstStepId,
                        Index = 1
                    },
                    new DbRecipeStep
                    {
                        Id = secondStepId,
                        Index = 2
                    }
                }
            });

            Action act = () => _sut.SwapSteps(firstStepId, secondStepId, recipeId);

            act.Should().NotThrow();
            var steps = _fixture.FindRecipe(recipeId).Steps;
            steps.First(step => step.Id == firstStepId).Index.Should().Be(2);
            steps.First(step => step.Id == secondStepId).Index.Should().Be(1);
        }

        [Fact]
        public void Editor_appends_a_step_without_ingredients()
        {
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            var newStep = new RecipeStep(Guid.NewGuid())
            {
                Description = "Текст",
                Image = "Изображение"
            };

            Action act = () => _sut.AppendStep(recipeId, newStep);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.Should().HaveCount(1);
        }

        [Fact]
        public void Editor_appends_a_step_with_ingredients()
        {
            var recipeId = Guid.NewGuid();
            var ingredientName = "Существует";
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.NewGuid(), ingredientName));
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            var newStep = new RecipeStep(Guid.NewGuid())
            {
                Description = "Текст",
                Image = "Изображение",
                IngredientsDetails =
                {
                    new Kernel.Models.Recipes.StepIngredientDetails
                    {
                        Amount = 1,
                        Measure = Kernel.Models.Ingredients.Measures.Gramms,
                        IngredientName = ingredientName
                    }
                }
            };

            Action act = () => _sut.AppendStep(recipeId, newStep);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.Should().HaveCount(1);
            result.Steps.First().StepIngredientsLink.First().DbIngredient.Name.Should().Be(ingredientName);
        }

        [Fact]
        public void Editor_removes_a_step_from_recipe()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe { Id = recipeId });
            var newStep = new RecipeStep(stepId)
            {
                Description = "Текст",
                Image = "Изображение"
            };

            Action act = () => _sut.RemoveStep(recipeId, stepId);

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.Steps.Should().BeEmpty();
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
