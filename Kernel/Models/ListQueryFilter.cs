namespace KitProjects.MasterChef.Kernel.Models
{
    public class ListQueryFilter
    {
        public string SearchTerm { get; set; }
        public int Limit { get; set; }
        public int BatchNumber { get; set; } = 1;
    }
}
