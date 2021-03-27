namespace KitProjects.MasterChef.Kernel.Abstractions
{
    public interface ICommand<TCommand>
    {
        void Execute(TCommand command);
    }
}
