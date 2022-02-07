namespace KP.Cookbook.Cqrs
{
    public interface ICommandHandler<TCommand>
    {
        void Execute(TCommand command);
    }

    public interface ICommandHandler<TCommand, TResult>
    {
        TResult Execute(TCommand command);
    }
}
