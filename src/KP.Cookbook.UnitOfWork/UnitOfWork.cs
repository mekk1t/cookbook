using KP.Cookbook.Database;
using System.Data.Common;

namespace KP.Cookbook.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbConnection _connection;
        private DbTransaction _transaction;

        public UnitOfWork(Func<DbConnection> connectionFactory)
        {
            _connection = connectionFactory();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Rollback() => _transaction.Rollback();

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
