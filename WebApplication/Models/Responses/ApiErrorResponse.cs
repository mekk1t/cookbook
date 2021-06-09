﻿using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Models.Responses
{
    /// <summary>
    /// Ответ от сервера с информацией об ошибках.
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Список сообщений об ошибках.
        /// </summary>
        public IReadOnlyList<string> Messages { get; }

        public ApiErrorResponse(IReadOnlyList<string> messages)
        {
            Messages = messages;
        }
    }
}