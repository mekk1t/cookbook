namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class CreateCategoryCommand
    {
        public string Name { get; }

        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }
}
