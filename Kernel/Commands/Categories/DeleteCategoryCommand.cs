using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class DeleteCategoryCommand
    {
        public Guid Id { get; }

        public DeleteCategoryCommand(Guid id) => Id = id;
    }
}
