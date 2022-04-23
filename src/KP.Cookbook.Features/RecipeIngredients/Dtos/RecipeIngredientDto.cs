using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Features.RecipeIngredients.Dtos
{
    public class RecipeIngredientDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public AmountType AmountType { get; set; }
        public bool IsOptional { get; set; }
    }
}
