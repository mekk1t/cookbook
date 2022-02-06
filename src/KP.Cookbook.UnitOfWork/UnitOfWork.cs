using KP.Cookbook.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbConnection _connection;
        private DbTransaction _transaction;

        public UnitOfWork(Func<DbConnection> connectionFactory)
        {
            _connection = connectionFactory();
            _transaction = _connection.BeginTransaction();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            if (_connection != null)
                _connection.Dispose();
        }

        public T Execute<T>(Func<DbConnection, DbTransaction, T> dbQuery) => dbQuery(_connection, _transaction);
    }
}
