using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Models.Responses
{
    /// <summary>
    /// Модель ответа от сервера с коллекцией данных.
    /// </summary>
    /// <typeparam name="T">Тип данных в коллекции.</typeparam>
    public class ApiCollectionResponse<T>
    {
        /// <summary>
        /// Отступ для получения данных далее по списку с текущими параметрами.
        /// </summary>
        public int? NextOffset { get; }
        /// <summary>
        /// Есть ли по текущим параметрам ещё данные.
        /// </summary>
        public bool? HasMoreItems { get; }
        /// <summary>
        /// Коллекция данных.
        /// </summary>
        public IEnumerable<T> Items { get; }

        public ApiCollectionResponse(IEnumerable<T> items) => Items = items ?? Array.Empty<T>();

        public ApiCollectionResponse(IEnumerable<T> items, int nextOffset, bool hasMoreItems)
        {
            Items = items ?? Array.Empty<T>();
            NextOffset = nextOffset;
            HasMoreItems = hasMoreItems;
        }
    }
}
