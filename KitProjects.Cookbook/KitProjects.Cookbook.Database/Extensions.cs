using KitProjects.Cookbook.Core.Models;

namespace KitProjects.Cookbook.Database
{
    public static class Extensions
    {
        /// <summary>
        /// Выбрасывает исключение, если объект сущности <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">Тип сущности, наследующийся от <see cref="Entity"/>.</typeparam>
        /// <exception cref="EntityNotFoundException"></exception>
        public static void ThrowIfNull<T>(this T entity) where T : Entity
        {
            if (entity == null)
                throw new EntityNotFoundException(typeof(T), entity.Id);
        }
    }
}
