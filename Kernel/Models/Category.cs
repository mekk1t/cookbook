using System;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class Category : Entity
    {
        public Category()
        {
        }

        public Category(Guid id) : base(id)
        {
        }

        public Category(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
