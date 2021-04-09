namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class SearchCategoryCommand
    {
        public string SearchTerm { get; }

        public SearchCategoryCommand(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
