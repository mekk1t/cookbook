namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class DeleteCategoryCommand
    {
        public string Name { get; }

        public DeleteCategoryCommand(string name)
        {
            Name = name;
        }
    }
}
