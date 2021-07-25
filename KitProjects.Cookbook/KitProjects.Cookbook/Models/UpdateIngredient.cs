using System.Collections.Generic;

namespace KitProjects.Cookbook.Models
{
    /// <summary>
    /// Запрос на обновление ингредиента.
    /// </summary>
    public class UpdateIngredient
    {
        /// <summary>
        /// ID ингредиента. Если указывается, будет отредактирован. Иначе - создан.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Название ингредиента. Задается новому ингредиенту или перезаписывает старому.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список категорий, к которым относится ингредиент.
        /// </summary>
        public List<UpdateCategory> Categories { get; set; } = new List<UpdateCategory>();
    }
}
