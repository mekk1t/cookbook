using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Dal.Database.Models
{
    public class DbCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; internal set; }
        public IEnumerable<DbIngredient> Ingredients { get; set; }
        public IEnumerable<DbRecipeCategory> RecipesCategoriesLink { get; set; }

        public DbCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
