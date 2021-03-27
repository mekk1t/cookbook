using System;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; internal set; }

        public DbCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
