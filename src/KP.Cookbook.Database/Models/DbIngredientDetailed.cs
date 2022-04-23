using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Database.Models
{
    public class DbIngredientDetailed
    {
        public long IngredientId { get; set; }
        public string Name { get; set; }
        public IngredientType Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public AmountType AmountType { get; set; }
        public bool IsOptional { get; set; }
    }
}
