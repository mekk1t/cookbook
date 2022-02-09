using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;

namespace KP.Cookbook.Features.Sources.UpdateSource
{
    public class UpdateSourceCommandHandler : ICommandHandler<UpdateSourceCommand>
    {
        private readonly SourcesRepository _repository;

        public UpdateSourceCommandHandler(SourcesRepository repository)
        {
            _repository = repository;
        }

        public void Execute(UpdateSourceCommand command) => _repository.Update(command.UpdatedSource);
    }
}
