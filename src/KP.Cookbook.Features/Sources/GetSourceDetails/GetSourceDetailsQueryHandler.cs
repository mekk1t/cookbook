using KP.Cookbook.Cqrs;
using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Sources.GetSourceDetails
{
    public class GetSourceDetailsQueryHandler : IQueryHandler<GetSourceDetailsQuery, Source>
    {
        private readonly SourcesRepository _repository;

        public GetSourceDetailsQueryHandler(SourcesRepository repository)
        {
            _repository = repository;
        }

        public Source Execute(GetSourceDetailsQuery query) => _repository.GetById(query.SourceId);
    }
}
