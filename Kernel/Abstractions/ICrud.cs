namespace KitProjects.MasterChef.Kernel.Abstractions
{
    /// <summary>
    /// CRUD-операции над сущностью <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICrud<TEntity, TKey> where TEntity : class
    {
        TEntity Create(TEntity entity);
        TEntity Read(TKey key);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
