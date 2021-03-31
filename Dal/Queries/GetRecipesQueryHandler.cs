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
                return _dbContext.Recipes.AsNoTracking()
                    .Include(r => r.RecipeCategoriesLink).ThenInclude(c => c.DbCategory)
                    .Include(r => r.Steps).ThenInclude(s => s.StepIngredientsLink).ThenInclude(si => si.DbIngredient)
                    .Include(r => r.RecipeIngredientLink).ThenInclude(ri => ri.DbIngredient)
                    .Select(r => new Recipe(r.Id)
                    {
                        Title = r.Title,
                        Description = r.Description,
                        Categories = r.RecipeCategoriesLink.Select(rc => new Category(rc.DbCategoryId, rc.DbCategory.Name)).ToList(),
                        Ingredients = r.RecipeIngredientLink.Select(ri => new Ingredient(ri.DbIngredientId, ri.DbIngredient.Name)).ToList(),
                        Steps = r.Steps.Select(s => new RecipeStep(s.Id)
                        {
                            Index = s.Index,
                            Description = s.Description,
                            Image = s.Image,
                            IngredientsDetails = s.StepIngredientsLink.Select(si => new StepIngredientDetails
                            {
                                Amount = si.Amount,
                                IngredientName = si.DbIngredient.Name,
                                Measure = si.Measure
                            })
                        }).ToList()
                    })
                    .OrderBy(r => r.Title)
                    .Skip(query.Offset)
                    .Take(query.Limit)
                    .AsEnumerable();
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
