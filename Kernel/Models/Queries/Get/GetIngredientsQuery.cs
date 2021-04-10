namespace KitProjects.MasterChef.Kernel.Models.Queries
{
    public class GetIngredientsQuery
    {
        public bool WithRelationships { get; }
        public int Limit { get; }
        public int Offset { get; }

        public GetIngredientsQuery(bool withRelationships = false, int limit = 25, int offset = 0)
        {
            Limit = limit;
            Offset = offset;
            WithRelationships = withRelationships;
        }
    }
}
