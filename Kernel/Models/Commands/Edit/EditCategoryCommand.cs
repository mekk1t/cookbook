using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class EditCategoryCommand
    {
        public Guid Id { get; }
        public string NewName { get; }

        public EditCategoryCommand(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}
