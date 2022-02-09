using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Sources.CreateSource
{
    public class CreateSourceCommand
    {
        public Source NewSource { get; }

        public CreateSourceCommand(Source newSource)
        {
            NewSource = newSource;
        }
    }
}
