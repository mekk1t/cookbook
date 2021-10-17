namespace KP.Cookbook.Domain.Models
{
    public enum IngredientMeasure
    {
        /// <summary>
        /// Ничего - когда меру не подобрать.
        /// </summary>
        ничего = 0,
        /// <summary>
        /// Граммы.
        /// </summary>
        гр,
        /// <summary>
        /// Килограммы.
        /// </summary>
        кг,
        /// <summary>
        /// Штуки.
        /// </summary>
        шт,
        /// <summary>
        /// Миллилитры.
        /// </summary>
        мл,
        /// <summary>
        /// Литры.
        /// </summary>
        л,
        /// <summary>
        /// Ложки (столовые).
        /// </summary>
        стл,
        /// <summary>
        /// Ложки (чайные).
        /// </summary>
        чл
    }
}
