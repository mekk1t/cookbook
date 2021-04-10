﻿using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Steps;
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
            _sut = new RecipeStepEditor(
                new EditStepPictureCommandHandler(editDbContext),
                new EditStepDescriptionCommandHandler(editDbContext),
                new SearchStepQueryHandler(queryDbContext));

            _dbContexts.AddRange(new[] { queryDbContext, editDbContext });
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

        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
