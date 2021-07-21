namespace KitProjects.Cookbook.Core.Abstractions
{
    public interface ICrud<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        TEntity Create(TEntity entity);
        TEntity Read(TKey key);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
