namespace KitProjects.MasterChef.Kernel.Models.Queries
{
    public class SearchIngredientQuery
    {
        public string SearchTerm { get; }

        public SearchIngredientQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
