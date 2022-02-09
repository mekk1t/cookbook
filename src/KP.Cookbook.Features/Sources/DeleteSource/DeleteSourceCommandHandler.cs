using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Sources.DeleteSource
{
    public class DeleteSourceCommandHandler : ICommandHandler<DeleteSourceCommand>
    {
        private readonly SourcesRepository _repository;

        public DeleteSourceCommandHandler(SourcesRepository repository)
        {
            _repository = repository;
        }

        public void Execute(DeleteSourceCommand command) => _repository.DeleteById(command.SourceId);
    }
}
