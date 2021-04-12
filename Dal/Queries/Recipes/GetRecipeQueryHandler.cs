using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Dal.Queries.Recipes
{
    public class GetRecipeQueryHandler : IQuery<RecipeDetails, GetRecipeQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetRecipeQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RecipeDetails Execute(GetRecipeQuery query)
        {
            var recipe = _dbContext.Recipes
                .AsNoTracking()
                .Include(r => r.Steps).ThenInclude(step => step.StepIngredientsLink).ThenInclude(link => link.DbIngredient)
                .Include(r => r.RecipeCategoriesLink).ThenInclude(r => r.DbCategory)
                .Include(r => r.RecipeIngredientLink).ThenInclude(r => r.DbIngredient)
                .FirstOrDefault(r => r.Id == query.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var result = new RecipeDetails
            {
                Id = recipe.Id,
                Description = recipe.Description,
                Title = recipe.Title
            };
            result.Categories.AddRange(recipe.RecipeCategoriesLink.Select(link => new Category(link.DbCategory.Id, link.DbCategory.Name)));
            result.Steps.AddRange(recipe.Steps.Select(dbstep =>
            {
                var step = new RecipeStep(dbstep.Id)
                {
                    Description = dbstep.Description,
                    Index = dbstep.Index,
                    Image = dbstep.Image,
                };
                step.IngredientsDetails.AddRange(dbstep.StepIngredientsLink.Select(link => new StepIngredientDetails
                {
                    Amount = link.Amount,
                    IngredientName = link.DbIngredient.Name,
                    Measure = link.Measure
                }));

                return step;
            }));
            result.Ingredients.AddRange(recipe.RecipeIngredientLink.Select(link =>
                new RecipeIngredientDetails(
                    link.DbIngredient.Name,
                    link.IngredientMeasure,
                    link.IngredientxAmount,
                    link.Notes,
                    link.DbIngredientId)));

            return result;
        }
    }
}
