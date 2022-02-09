namespace KP.Cookbook.Features.Sources.GetSourceDetails
{
    public class GetSourceDetailsQuery
    {
        public long SourceId { get; }

        public GetSourceDetailsQuery(long sourceId)
        {
            SourceId = sourceId;
        }
    }
}
