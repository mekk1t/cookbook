using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class CreateRecipeCommandHandler : ICommand<CreateRecipeCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreateRecipeCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(CreateRecipeCommand command)
        {
            var categories = _dbContext.Categories.Where(c => command.Categories.Contains(c.Name)).AsEnumerable();
            var ingredients = _dbContext.Ingredients.Where(i => command.IngredientsDetails.Select(c => c.IngredientName).Contains(i.Name)).AsEnumerable();
            var newRecipe = new DbRecipe
            {
                Id = command.Id,
                Title = command.Title,
                Description = command.Description,
                RecipeCategoriesLink = categories.Select(c => new DbRecipeCategory
                {
                    DbRecipeId = command.Id,
                    DbCategoryId = c.Id
                }).ToList(),
                RecipeIngredientLink = ingredients.Select(c => new DbRecipeIngredient
                {
                    DbRecipeId = command.Id,
                    DbIngredientId = c.Id,
                    IngredientMeasure = command.IngredientsDetails.First(details => details.IngredientName == c.Name).Measure,
                    IngredientsAmount = command.IngredientsDetails.First(details => details.IngredientName == c.Name).Amount,
                    Notes = command.IngredientsDetails.First(details => details.IngredientName == c.Name).Notes
                }).ToList(),
                Steps = command.Steps.Select(step => new DbRecipeStep
                {
                    Id = step.Id,
                    Description = step.Description,
                    Image = step.Image,
                    Index = step.Index,
                    StepIngredientsLink = step.IngredientsDetails.Select(i => new DbRecipeStepIngredient
                    {
                        DbRecipeStepId = step.Id,
                        DbIngredientId = ingredients.First(ingredient => ingredient.Name == i.IngredientName).Id,
                        Amount = i.Amount,
                        Measure = i.Measure
                    }).ToList()
                }).ToList()
            };

            _dbContext.Recipes.Add(newRecipe);
            _dbContext.SaveChanges();
        }
    }
}
