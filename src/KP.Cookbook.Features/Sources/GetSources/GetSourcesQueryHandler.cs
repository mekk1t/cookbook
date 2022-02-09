using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Sources.GetSources
{
    public class GetSourcesQueryHandler : IQueryHandler<GetSourcesQuery, List<Source>>
    {
        private readonly SourcesRepository _repository;

        public GetSourcesQueryHandler(SourcesRepository repository)
        {
            _repository = repository;
        }

        public List<Source> Execute(GetSourcesQuery query) => _repository.Get();
    }
}
