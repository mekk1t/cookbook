using System;

namespace KitProjects.MasterChef.Kernel.Models.Queries.Search
{
    public class SearchStepQueryParameters
    {
        public Guid RecipeId { get; }
        public int Index { get; }

        public SearchStepQueryParameters(Guid recipeId, int index)
        {
            RecipeId = recipeId;
            Index = index;
        }
    }
}
