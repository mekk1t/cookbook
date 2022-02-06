﻿using KP.Cookbook.Database;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Uow;

namespace KP.Cookbook.RestApi
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

        public Ingredient Create(Ingredient ingredient)
        {
            try
            {
                var result = _repository.Create(ingredient);
                _unitOfWork.Commit();
                return result;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public List<Ingredient> Get() => _repository.Get();
    }
}
