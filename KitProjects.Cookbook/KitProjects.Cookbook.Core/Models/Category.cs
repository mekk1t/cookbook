using System.Collections.Generic;
using System.Linq;

namespace KitProjects.Cookbook.Core.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public List<Ingredient> Ingredients { get; } = new List<Ingredient>();

        public Category()
        {

        }

        public Category(long id) : base(id)
        {

        }

        /// <summary>
        /// Копирует данные из другого объекта категории.
        /// </summary>
        /// <remarks>
        /// Ингредиенты также копируются, если есть.
        /// </remarks>
        /// <param name="other"></param>
        public Category(Category other) : base(other.Id)
        {
            Name = other.Name;
            Type = other.Type;
            Ingredients = other.Ingredients.Select(i => new Ingredient(i)).ToList();
        }
    }
}
