using KP.Cookbook.Cqrs;

namespace KP.Cookbook.RestApi.Uow
{
    public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly UnitOfWork _uow;

        public UnitOfWorkCommandHandlerDecorator(UnitOfWork uow, ICommandHandler<TCommand> handler)
        {
            _uow = uow;
            _handler = handler;
        }

        public void Execute(TCommand command)
        {
            try
            {
                _handler.Execute(command);
                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }
    }

    public class UnitOfWorkCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly UnitOfWork _uow;

        public UnitOfWorkCommandHandlerDecorator(UnitOfWork uow, ICommandHandler<TCommand, TResult> handler)
        {
            _uow = uow;
            _handler = handler;
        }

        public TResult Execute(TCommand command)
        {
            try
            {
                var result = _handler.Execute(command);
                _uow.Commit();
                return result;
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }
    }
}
