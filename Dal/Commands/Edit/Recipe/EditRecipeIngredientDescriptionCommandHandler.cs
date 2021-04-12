using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands.Edit.Recipe
{
    public class EditRecipeIngredientDescriptionCommandHandler : ICommand<EditRecipeIngredientDescriptionCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditRecipeIngredientDescriptionCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditRecipeIngredientDescriptionCommand command)
        {
            var recipe = _dbContext.Recipes
                .Include(r => r.RecipeIngredientLink)
                .FirstOrDefault(r => r.Id == command.RecipeId);
            if (recipe == null)
                throw new EntityNotFoundException();

            var editLink = recipe.RecipeIngredientLink.FirstOrDefault(link => link.DbIngredientId == command.IngredientId);
            if (editLink == null)
                throw new ArgumentException(null, nameof(command));

            if (command.Amount != default)
                editLink.IngredientxAmount = command.Amount;
            if (command.Measure != Kernel.Models.Ingredients.Measures.None)
                editLink.IngredientMeasure = command.Measure;
            if (command.Notes.IsNotNullOrEmpty())
                editLink.Notes = command.Notes;

            _dbContext.SaveChanges();
        }
    }
}
