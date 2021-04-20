using System;

namespace KitProjects.MasterChef.Kernel.Models.Queries
{
    public class SearchIngredientQuery
    {
        public string SearchTerm { get; }
        public Guid IngredientId { get; }

        public SearchIngredientQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public SearchIngredientQuery(Guid ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
