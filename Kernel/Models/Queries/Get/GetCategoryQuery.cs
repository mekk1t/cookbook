using System;

namespace KitProjects.MasterChef.Kernel.Models.Queries.Get
{
    public class GetCategoryQuery
    {
        public string CategoryName { get; }
        public Guid CategoryId { get; }

        public GetCategoryQuery(string categoryName)
        {
            CategoryName = categoryName;
        }

        public GetCategoryQuery(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
