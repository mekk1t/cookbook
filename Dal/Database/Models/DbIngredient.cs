using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbIngredient
    {
        public Guid Id { get; private set; }
        public string Name { get; internal set; }
        public ICollection<DbCategory> Categories { get; internal set; }

        public DbIngredient(Guid id, string name, ICollection<DbCategory> categories) : this(id, name)
        {
            Categories = categories;
        }

        private DbIngredient(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
