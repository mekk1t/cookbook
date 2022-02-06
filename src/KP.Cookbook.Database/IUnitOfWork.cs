using System.Data.Common;

namespace KP.Cookbook.Database
{
    public interface IUnitOfWork
    {
        T Execute<T>(Func<DbConnection, DbTransaction, T> dbQuery);
    }
}
