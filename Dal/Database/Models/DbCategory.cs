﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; internal set; }
        public ICollection<DbIngredient> Ingredients { get; private set; }

        public DbCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
