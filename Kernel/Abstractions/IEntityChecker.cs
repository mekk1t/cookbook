namespace KitProjects.MasterChef.Kernel.Abstractions
{
    public interface IEntityChecker<in TEntity, TParameters>
        where TEntity : Entity
    {
        bool CheckExistence(TParameters parameters);
    }
}
