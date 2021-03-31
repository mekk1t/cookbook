using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Extensions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Dal.Commands
{
    public class EditRecipeCommandHandler : ICommand<EditRecipeCommand>
    {
        private readonly AppDbContext _dbContext;

        public EditRecipeCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Execute(EditRecipeCommand command)
        {
            var oldRecipe = _dbContext.Recipes.FirstOrDefault(r => r.Id == command.RecipeId);
            if (oldRecipe == null)
                throw new ArgumentException($"Рецепт с ID {command.RecipeId} не существует.");

            oldRecipe.Title = command.NewTitle.IsNullOrEmpty()
                ? oldRecipe.Title
                : command.NewTitle;
            oldRecipe.Description = command.NewDescription.IsNullOrEmpty()
                ? oldRecipe.Description
                : command.NewDescription;

            _dbContext.SaveChanges();
        }
    }
}
