using Dapper;
using KP.Cookbook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.Database
{
    public class IngredientsRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;

        public IngredientsRepository(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            _dbConnection = dbConnection;
            _dbTransaction = dbTransaction;
        }

        public Task Create(Ingredient ingredient)
        {
            var sql = "";
            var parameters = new { };

            return _dbConnection.ExecuteAsync(new CommandDefinition(sql, parameters, _dbTransaction));
        }
    }
}
