using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Domain.Entities
{
    public class CookingStep : Entity
    {
        public int Order { get; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<IngredientDetailed> Ingredients { get; } = new List<IngredientDetailed>();

        public CookingStep(long id, int order) : base(id)
        {
            if (order <= 0)
                throw new InvariantException("Не указан порядковый номер шага");

            Order = order;
        }

        public CookingStep(int order)
        {
            if (order <= 0)
                throw new InvariantException("Не указан порядковый номер шага");

            Order = order;
        }
    }
}
