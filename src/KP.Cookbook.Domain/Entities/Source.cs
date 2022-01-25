namespace KP.Cookbook.Domain.Entities
{
    public class Source : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Link { get; set; }
        public string? Image { get; set; }
    }
}
