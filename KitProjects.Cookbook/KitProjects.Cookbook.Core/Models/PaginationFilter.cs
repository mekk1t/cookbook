namespace KitProjects.Cookbook.Core.Models
{
    /// <summary>
    /// Фильтры пагинации.
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Максимальное количество элементов в ответе.
        /// </summary>
        public int Limit { get; set; } = 10;
        /// <summary>
        /// ID последней записи в текущей выборке.
        /// </summary>
        public int LastId { get; set; }
    }
}
