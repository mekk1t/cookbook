using System;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class CategoryIngredientViewModel
    {
        /// <summary>
        /// Идентификатор ингредиента в формате GUID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string Name { get; }

        public CategoryIngredientViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
