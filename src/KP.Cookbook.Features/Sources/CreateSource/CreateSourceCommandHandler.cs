using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Sources.CreateSource
{
    public class CreateSourceCommandHandler : ICommandHandler<CreateSourceCommand, Source>
    {
        private readonly SourcesRepository _repository;

        public CreateSourceCommandHandler(SourcesRepository repository)
        {
            _repository = repository;
        }

        public Source Execute(CreateSourceCommand command) => _repository.Create(command.NewSource);
    }
}
