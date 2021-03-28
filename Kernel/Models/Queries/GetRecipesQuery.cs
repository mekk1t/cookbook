namespace KitProjects.MasterChef.Kernel.Models.Queries
{
    public class GetRecipesQuery
    {
        public bool WithRelationships { get; }
        public int Limit { get; }
        public int Offset { get; }

        public GetRecipesQuery(bool withRelationships = false, int limit = 25, int offset = 0)
        {
            Limit = limit;
            Offset = offset;
            WithRelationships = withRelationships;
        }
    }
}
