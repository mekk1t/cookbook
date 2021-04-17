using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class GetSingleIngredientResponse
    {
        /// <summary>
        /// ID ингредиента в формате GUID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Список категорий ингредиента.
        /// </summary>
        public IEnumerable<Category> Categories { get; }

        public GetSingleIngredientResponse(Guid id, string name, IEnumerable<Category> categories)
        {
            Id = id;
            Name = name;
            Categories = categories;
        }
    }
}
