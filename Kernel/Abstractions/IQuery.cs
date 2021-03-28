namespace KitProjects.MasterChef.Kernel.Abstractions
{
    public interface IQuery<out TResult>
    {
        TResult Execute();
    }

    public interface IQuery<out TResult, TQuery>
    {
        TResult Execute(TQuery query);
    }
}
