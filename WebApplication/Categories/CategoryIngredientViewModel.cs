using System;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoryIngredientViewModel
    {
        public Guid Id { get; }
        public string Name { get; }

        public CategoryIngredientViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
