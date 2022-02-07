using KP.Cookbook.Database;
using KP.Cookbook.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.RestApi.Services
{
    public class UsersService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UsersRepository _repository;

        public UsersService(UnitOfWork unitOfWork, UsersRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }


    }
}
