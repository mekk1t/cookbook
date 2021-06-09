using System;

namespace KitProjects.MasterChef.WebApplication.Models.Responses.Categories
{
    /// <summary>
    /// Краткая информация о категории.
    /// </summary>
    public class CategoryShortResponse
    {
        /// <summary>
        /// ID категории в формате GUID.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; }

        public CategoryShortResponse(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
