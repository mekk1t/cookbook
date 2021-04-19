using FluentAssertions;
using KitProjects.Fixtures;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
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
                    null,
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
                                    new GetCategoryQueryHandler(_dbContext))))));
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

        public void Dispose() => _dbContext.Dispose();
    }
}
