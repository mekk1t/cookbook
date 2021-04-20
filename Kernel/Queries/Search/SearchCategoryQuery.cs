namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class SearchCategoryQuery
    {
        public string SearchTerm { get; }

        public SearchCategoryQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
