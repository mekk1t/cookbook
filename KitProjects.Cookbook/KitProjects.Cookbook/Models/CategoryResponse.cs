using KitProjects.Cookbook.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace KitProjects.Cookbook.Models
{
    /// <summary>
    /// Информация о категории.
    /// </summary>
    public class CategoryResponse
    {
        /// <summary>
        /// ID категории в числовом формате.
        /// </summary>
        public long Id { get; }
        /// <summary>
        /// Название категории.
        /// </summary>
        [Required]
        public string Name { get; }
        /// <summary>
        /// Тип категории: ингредиент или рецепт.
        /// </summary>
        [Required]
        public CategoryType Type { get; }

        public CategoryResponse(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Id = category.Id;
            Name = category.Name;
            Type = category.Type;
        }
    }
}
