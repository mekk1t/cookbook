using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class SearchRecipeCommand
    {
        public string SearchTerm { get; }
        public Guid RecipeId { get; }

        public SearchRecipeCommand(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public SearchRecipeCommand(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
