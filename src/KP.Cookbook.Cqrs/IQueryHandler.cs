namespace KP.Cookbook.Cqrs
{
    public interface IQueryHandler<TQuery, TResult>
    {
        TResult Execute(TQuery query);
    }
}
