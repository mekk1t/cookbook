using System;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class GetCategoryResponse
    {
        /// <summary>
        /// Название категории.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// ID категории в формате GUID.
        /// </summary>
        public Guid Id { get; }

        public GetCategoryResponse(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
