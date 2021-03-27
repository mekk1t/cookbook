using System;

namespace KitProjects.MasterChef.WebApplication.Categories
{
    public class GetCategoryResponse
    {
        public string Name { get; }
        public Guid Id { get; }

        public GetCategoryResponse(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
