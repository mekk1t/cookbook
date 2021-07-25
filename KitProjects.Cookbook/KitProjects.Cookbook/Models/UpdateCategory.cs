using System.ComponentModel.DataAnnotations;

namespace KitProjects.Cookbook.Models
{
    /// <summary>
    /// Модель обновления категории.
    /// </summary>
    public class UpdateCategory
    {
        /// <summary>
        /// ID категории. Опускать для создания, указывать для редактирования.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Название категории. Перезапишет старое при редактировании или задастся новой при создании.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
