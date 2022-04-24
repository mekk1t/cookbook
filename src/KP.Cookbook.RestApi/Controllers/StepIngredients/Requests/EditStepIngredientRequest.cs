using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.RestApi.Controllers.StepIngredients.Requests
{
    public class EditStepIngredientRequest
    {
        public long IngredientId { get; set; }
        public decimal Amount { get; set; }
        public AmountType AmountType { get; set; }
        public bool IsOptional { get; set; }
    }
}
