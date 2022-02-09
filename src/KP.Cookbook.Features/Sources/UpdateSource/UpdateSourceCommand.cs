using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Sources.UpdateSource
{
    public class UpdateSourceCommand
    {
        public Source UpdatedSource { get; }

        public UpdateSourceCommand(Source updatedSource)
        {
            UpdatedSource = updatedSource;
        }
    }
}
