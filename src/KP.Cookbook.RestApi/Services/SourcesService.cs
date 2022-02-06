using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Uow;

namespace KP.Cookbook.RestApi.Services
{
    public class SourcesService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly SourcesRepository _repository;

        public SourcesService(UnitOfWork unitOfWork, SourcesRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public Source? Create(Source source)
        {
            Source? result = null;
            UnitOfWorkWrap(() => result = _repository.Create(source));
            return result;
        }

        public List<Source> Get() => _repository.Get();

        public void Update(Source source) => UnitOfWorkWrap(() => _repository.Update(source));

        public void Delete(long id) => UnitOfWorkWrap(() => _repository.DeleteById(id));

        private void UnitOfWorkWrap(Action action)
        {
            try
            {
                action();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
