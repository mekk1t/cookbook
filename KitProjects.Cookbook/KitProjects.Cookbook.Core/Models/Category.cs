namespace KitProjects.Cookbook.Core.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public CategoryType Type { get; set; }

        public Category()
        {

        }

        public Category(long id) : base(id)
        {

        }
    }
}
