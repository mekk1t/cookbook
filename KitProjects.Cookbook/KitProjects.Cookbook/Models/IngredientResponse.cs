using KitProjects.Cookbook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitProjects.Cookbook.Models
{
    /// <summary>
    /// Информация об ингредиенте.
    /// </summary>
    public class IngredientResponse
    {
        /// <summary>
        /// ID ингредиента в числовом формате.
        /// </summary>
        public long Id { get; }
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Категории ингредиента.
        /// </summary>
        public List<CategoryResponse> Categories { get; }

        /// <summary>
        /// Представляет сущность <see cref="Ingredient"/> в формате API-ответа <see cref="IngredientResponse"/>.
        /// </summary>
        /// <param name="ingredient"></param>
        public IngredientResponse(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient));

            Id = ingredient.Id;
            Name = ingredient.Name;
            Categories = ingredient.Categories.Select(c => new CategoryResponse(c)).ToList();
        }
    }
}
