using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class SearchRecipeQuery
    {
        public string SearchTerm { get; }
        public Guid RecipeId { get; }

        public SearchRecipeQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public SearchRecipeQuery(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
