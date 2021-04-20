﻿using FluentAssertions;
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
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.EntityChecks;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KitProjects.MasterChef.Tests.Commands
{
    [Collection("Db")]
    public class ReplaceStepIngredientCommandHandlerTests : IDisposable
    {
        private readonly DbFixture _fixture;
        private readonly AppDbContext _dbContext;
        private readonly ICommand<ReplaceStepIngredientCommand> _sut;

        public ReplaceStepIngredientCommandHandlerTests(DbFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
            var getRecipe = new GetRecipeQueryHandler(_dbContext);
            var ingredientChecker = new IngredientChecker(new GetIngredientQueryHandler(_dbContext));
            _sut =
                new ReplaceStepIngredientDecorator(
                    new ReplaceStepIngredientCommandHandler(_dbContext),
                    getRecipe,
                    new AppendIngredientToRecipeDecorator(
                        new AppendIngredientCommandHandler(_dbContext),
                        new RecipeChecker(getRecipe),
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
        public void Cant_replace_the_same_ingredients()
        {
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeIngredientLink = new List<DbRecipeIngredient>
                {
                    new DbRecipeIngredient
                    {
                        DbRecipeId = recipeId,
                        DbIngredientId = ingredientId
                    }
                },
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

            Action act = () => _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId, ingredientId.ToString()),
                    new Ingredient(ingredientId, ingredientId.ToString())));

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Cant_replace_step_ingredient_in_nonexistent_recipe()
        {
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));

            Action act = () => _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId.ToString()),
                    new Ingredient(ingredientId.ToString())));

            act.Should().ThrowExactly<EntityNotFoundException>();
        }

        [Fact]
        public void Cant_replace_step_ingredient_in_nonexistent_recipe_step()
        {
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId
            });
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));

            Action act = () => _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId.ToString()),
                    new Ingredient(ingredientId.ToString())));

            act.Should().ThrowExactly<EntityNotFoundException>();
        }

        [Fact]
        public void Cant_replace_nonexistent_ingredient_from_step()
        {
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeIngredientLink = new List<DbRecipeIngredient>
                {
                    new DbRecipeIngredient
                    {
                        DbRecipeId = recipeId,
                        DbIngredientId = ingredientId
                    }
                },
                Steps = new List<DbRecipeStep>
                {
                    new DbRecipeStep
                    {
                        Id = stepId
                    }
                }
            });

            Action act = () => _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId, ingredientId.ToString()),
                    new Ingredient(Guid.NewGuid().ToString())));

            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Recipe_has_a_replacement_ingredient_appended()
        {
            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var newIngredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeIngredientLink = new List<DbRecipeIngredient>
                {
                    new DbRecipeIngredient
                    {
                        DbRecipeId = recipeId,
                        DbIngredientId = ingredientId
                    }
                },
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

            _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId, ingredientId.ToString()),
                    new Ingredient(newIngredientName)));

            var result = _fixture.FindRecipe(recipeId);
            result.Steps
                .First(step => step.Id == stepId).StepIngredientsLink
                    .First().DbIngredient.Name
                    .Should()
                    .Be(newIngredientName);
            result.RecipeIngredientLink.First().DbIngredient.Name.Should().Be(newIngredientName);
        }

        [Fact]
        public void Recipe_step_gets_ingredient_replaced()
        {

            var ingredientId = Guid.NewGuid();
            var recipeId = Guid.NewGuid();
            var stepId = Guid.NewGuid();
            var newIngredientName = Guid.NewGuid().ToString();
            _fixture.SeedIngredientWithNewCategories(new Ingredient(ingredientId, ingredientId.ToString()));
            _fixture.SeedRecipe(new DbRecipe
            {
                Id = recipeId,
                RecipeIngredientLink = new List<DbRecipeIngredient>
                {
                    new DbRecipeIngredient
                    {
                        DbRecipeId = recipeId,
                        DbIngredientId = ingredientId
                    }
                },
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

            _sut.Execute(
                new ReplaceStepIngredientCommand(
                    new RecipeStepIds(
                        recipeId,
                        stepId),
                    new Ingredient(ingredientId, ingredientId.ToString()),
                    new Ingredient(newIngredientName)));

            var result = _fixture.FindRecipe(recipeId);
            result.Steps
                .First(step => step.Id == stepId).StepIngredientsLink
                    .First().DbIngredient.Name
                    .Should()
                    .Be(newIngredientName);
        }

        public void Dispose() => _dbContext.Dispose();
    }
}