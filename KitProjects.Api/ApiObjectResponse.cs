namespace KitProjects.MasterChef.WebApplication.Models.Responses
{
    /// <summary>
    /// Модель ответа от сервера с данными об объекте.
    /// </summary>
    /// <typeparam name="T">Тип данных в ответе сервера.</typeparam>
    public class ApiObjectResponse<T>
    {
        /// <summary>
        /// Ответ от сервера.
        /// </summary>
        public T Data { get; }

        public ApiObjectResponse(T data)
        {
            Data = data;
        }
    }
}
