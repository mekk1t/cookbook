namespace KitProjects.MasterChef.WebApplication.Models.Filters
{
    public class PaginationFilter
    {
        public bool WithRelationships { get; set; } = false;
        public int Limit { get; set; } = 25;
        public int Offset { get; set; }
    }
}
