using KitProjects.MasterChef.Dal.Database.Models;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                //Categories = categories.ToList(),
                Steps = command.Steps.Select(step => new DbRecipeStep
                {
                    Id = step.Id,
                    Description = step.Description,
                    Image = step.Image,
                    Index = step.Index,
                }).ToList()
            };
        }
    }
}
