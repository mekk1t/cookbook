using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Recipes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class GetRecipesQueryHandler : IQuery<IEnumerable<Recipe>, GetRecipesQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetRecipesQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Recipe> Execute(GetRecipesQuery query)
        {
            if (query.WithRelationships)
            {
                var recipes = _dbContext.Recipes.AsNoTracking()
                    .Include(r => r.RecipeCategoriesLink).ThenInclude(c => c.DbCategory)
                    .Include(r => r.Steps).ThenInclude(s => s.StepIngredientsLink).ThenInclude(si => si.DbIngredient)
                    .Include(r => r.RecipeIngredientLink).ThenInclude(ri => ri.DbIngredient)
                    .OrderBy(r => r.Title)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .AsEnumerable();
                var result = recipes.Select(r =>
                {
                    var recipe = new Recipe(r.Id)
                    {
                        Title = r.Title,
                        Description = r.Description
                    };

                    recipe.Categories.AddRange(r.RecipeCategoriesLink.Select(rc => new Category(rc.DbCategoryId, rc.DbCategory.Name)));
                    recipe.Ingredients.AddRange(r.RecipeIngredientLink.Select(ri => new Ingredient(ri.DbIngredientId, ri.DbIngredient.Name)));
                    recipe.Steps.AddRange(r.Steps
                        .OrderBy(step => step.Index)
                        .Select(s =>
                    {
                        var step = new RecipeStep(s.Id)
                        {
                            Index = s.Index,
                            Description = s.Description,
                            Image = s.Image
                        };
                        step.IngredientsDetails.AddRange(s.StepIngredientsLink.Select(si => new StepIngredientDetails
                        {
                            Amount = si.Amount,
                            IngredientName = si.DbIngredient.Name,
                            Measure = si.Measure
                        }));
                        return step;
                    }));
                    return recipe;
                });
                return result;
            }

            return _dbContext.Recipes.AsNoTracking()
                    .Select(r => new Recipe(r.Id)
                    {
                        Title = r.Title,
                        Description = r.Description
                    })
                    .OrderBy(r => r.Title)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .AsEnumerable();
        }
    }
}
