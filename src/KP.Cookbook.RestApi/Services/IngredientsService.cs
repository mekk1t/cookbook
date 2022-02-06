using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Uow;

namespace KP.Cookbook.RestApi.Services
{
    public class IngredientsService
    {
        private readonly IngredientsRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public IngredientsService(IngredientsRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Ingredient? Create(Ingredient ingredient)
        {
            Ingredient? result = null;
            UnitOfWorkWrap(() => result = _repository.Create(ingredient));
            return result;
        }

        public List<Ingredient> Get() => _repository.Get();

        public void Update(Ingredient ingredient) => UnitOfWorkWrap(() => _repository.Update(ingredient));

        public void Delete(long id) => UnitOfWorkWrap(() => _repository.Delete(id));

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
