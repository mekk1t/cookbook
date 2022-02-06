namespace KP.Cookbook.Domain.Entities
{
    public class Source : Entity
    {
        public string Name { get; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? Image { get; set; }
        public bool IsApproved { get; set; }

        public Source(string name)
        {
            Name = name ?? throw new InvariantException("Отсутствует название у источника рецептов");
        }

        public Source(long id, string name) : base(id)
        {
            Name = name ?? throw new InvariantException("Отсутствует название у источника рецептов");
        }
    }
}
