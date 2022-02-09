namespace KP.Cookbook.Features.Sources.DeleteSource
{
    public class DeleteSourceCommand
    {
        public long SourceId { get; }

        public DeleteSourceCommand(long sourceId)
        {
            SourceId = sourceId;
        }
    }
}
