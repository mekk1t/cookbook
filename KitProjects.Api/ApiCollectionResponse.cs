using System;
using System.Collections.Generic;

namespace KitProjects.Api.AspNetCore
{
    /// <summary>
    /// Ответ от сервера для коллекции данных.
    /// </summary>
    /// <typeparam name="T">Тип данных в коллекции.</typeparam>
    public class ApiCollectionResponse<T>
    {
        /// <summary>
        /// Есть ли еще данные по текущему запросу.
        /// </summary>
        public bool? HasMoreItems { get; }
        /// <summary>
        /// Коллекция данных.
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Создает ответ от сервера для статической коллекции данных.
        /// </summary>
        /// <param name="items"></param>
        public ApiCollectionResponse(IEnumerable<T> items) => Items = items ?? Array.Empty<T>();

        /// <summary>
        /// Создает ответ от сервера для динамической коллекции данных с параметрами.
        /// </summary>
        /// <param name="items">Коллекция данных.</param>
        /// <param name="hasMoreItems">Есть ли еще данные по текущему запросу.</param>
        public ApiCollectionResponse(IEnumerable<T> items, bool hasMoreItems)
        {
            Items = items ?? Array.Empty<T>();
            HasMoreItems = hasMoreItems;
        }
    }
}
