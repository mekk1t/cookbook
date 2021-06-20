using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Commands.RecipeIngredients;
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Steps;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public sealed class AppendIngredientToStepTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly DbFixture _fixture;
        private readonly ICommand<AppendIngredientToStepCommand> _sut;

        public AppendIngredientToStepTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            var getRecipeQueryHandler = new GetRecipeQueryHandler(_dbContext);
            var ingredientChecker = new IngredientChecker(new GetIngredientQueryHandler(_dbContext));
            _sut =
                new AppendIngredientToStepDecorator(
                    new AppendIngredientToStepCommandHandler(_dbContext),
                    getRecipeQueryHandler,
                    new AppendIngredientToRecipeDecorator(
                        new AppendIngredientCommandHandler(_dbContext),
                        new RecipeChecker(getRecipeQueryHandler),
                        ingredientChecker,
                        new CreateIngredientDecorator(
                            new CreateIngredientCommandHandler(_dbContext),
                            ingredientChecker,
                            new CreateCategoryDecorator(
                                new CreateCategoryCommandHandler(_dbContext),
                                new CategoryChecker(
                                    new GetCategoryQueryHandler(_dbContext))))),
                    new GetIngredientQueryHandler(_dbContext));
        }

        [Fact]
        public void Cant_append_ingredient_to_step_in_nonexistent_recipe()
        {
            Action act = () => _sut.Execute(
                new AppendIngredientToStepCommand(
                    new RecipeStepIds(Guid.NewGuid(), Guid.NewGuid()),
                    null,
                    null));

            act.Should().ThrowExactly<EntityNotFoundException>();
        }

        [Fact]
        public void Cant_append_ingredient_to_nonexistent_step_in_recipe()
        {
            var recipeId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });

            Action act = () => _sut.Execute(
                new AppendIngredientToStepCommand(
                    new RecipeStepIds(recipeId, Guid.NewGuid()),
                    null,
                    null));

            act.Should().ThrowExactly<EntityNotFoundException>();
        }

        [Fact]
        public void Recipe_gets_ingredient_appended_when_step_adds_new_to_recipe_ingredient()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.Parse(ingredientName), ingredientName));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Index = 1
                    }
                }
            });

            Action act = () => _sut.Execute(new AppendIngredientToStepCommand(
                new RecipeStepIds(recipeId, stepId),
                new Ingredient(Guid.Parse(ingredientName), ingredientName),
                new IngredientParameters(0, Measures.ml)));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Select(link => link.DbIngredient.Name)
                .Should()
                .Contain(ingredientName);
            result.Steps
                .First(step => step.Id == stepId).StepIngredientsLink.Select(link => link.DbIngredient.Name)
                .Should()
                .Contain(ingredientName);
        }

        [Fact]
        public void Recipe_gets_newly_created_ingredient_appended_when_step_adds_totally_new_ingredient()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Index = 1
                    }
                }
            });

            Action act = () => _sut.Execute(new AppendIngredientToStepCommand(
                new RecipeStepIds(recipeId, stepId),
                new Ingredient(ingredientName),
                new IngredientParameters(0, Measures.ml)));

            act.Should().NotThrow();
            var result = _fixture.FindRecipe(recipeId);
            result.RecipeIngredientLink.Select(link => link.DbIngredient.Name)
                .Should()
                .Contain(ingredientName);
            result.Steps
                .First(step => step.Id == stepId).StepIngredientsLink.Select(link => link.DbIngredient.Name)
                .Should()
                .Contain(ingredientName);
        }

        [Fact]
        public void Cant_append_ingredient_that_is_already_in_that_step()
        {
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var ingredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(Guid.Parse(ingredientName), ingredientName));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId,
                        Index = 1,
                        StepIngredientsLink = new List<DbRecipeStepIngredient>
                        {
                            new DbRecipeStepIngredient
                            {
                                DbRecipeStepId = stepId,
                                DbIngredientId = Guid.Parse(ingredientName)
                            }
                        }
                    }
                }
            });

            Action act = () => _sut.Execute(new AppendIngredientToStepCommand(
                new RecipeStepIds(recipeId, stepId),
                new Ingredient(ingredientName),
                new IngredientParameters(0, Measures.ml)));

            act.Should().ThrowExactly<EntityDuplicateException>();
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
