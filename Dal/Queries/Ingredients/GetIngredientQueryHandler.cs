using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Queries.Ingredients
{
    public class GetIngredientQueryHandler : IQuery<Ingredient, GetIngredientQuery>
    {
        private readonly AppDbContext _dbContext;

        public GetIngredientQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Ingredient Execute(GetIngredientQuery query)
        {
            DbIngredient ingredient;
            if (query.IngredientId != Guid.Empty)
            {
                ingredient = _dbContext.Ingredients.AsNoTracking().FirstOrDefault(i => i.Id == query.IngredientId);
            }
            else if (query.IngredientName.IsNotNullOrEmpty())
            {
                ingredient = _dbContext.Ingredients.AsNoTracking().FirstOrDefault(i => i.Name == query.IngredientName);
            }
            else
            {
                throw new ArgumentException(null, nameof(query));
            }

            if (ingredient == null)
                return null;

            return new Ingredient(ingredient.Id, ingredient.Name);
        }
    }
}
