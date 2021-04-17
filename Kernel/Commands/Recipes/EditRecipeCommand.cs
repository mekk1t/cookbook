using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class EditRecipeCommand
    {
        public Guid RecipeId { get; }
        public string NewTitle { get; }
        public string NewDescription { get; }

        public EditRecipeCommand(Guid recipeId, string newTitle, string newDescription)
        {
            RecipeId = recipeId;
            NewTitle = newTitle;
            NewDescription = newDescription;
        }
    }
}
