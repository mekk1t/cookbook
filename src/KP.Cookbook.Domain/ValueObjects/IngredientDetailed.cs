using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Domain.ValueObjects
{
    public class IngredientDetailed : ValueObject
    {
        public Ingredient Ingredient { get; }
        public decimal Amount { get; }
        public AmountType AmountType { get; }
        public bool IsOptional { get; set; } = true;

        public IngredientDetailed(Ingredient ingredient, decimal amount, AmountType amountType)
        {
            Ingredient = ingredient ?? throw new InvariantException("Ингредиент не может быть пустым.");

            if (AmountMeasurementsAreIncorrect(amount, amountType))
                throw new InvariantException("Не задано измерение ингредиента.");

            Amount = Math.Round(amount, 1);
            AmountType = amountType;
        }

        private static bool AmountMeasurementsAreIncorrect(decimal amount, AmountType amountType)
        {
            bool typeIsNotSpecified = amount != 0 && amountType == AmountType.None;
            bool amountIsNotSpecified = amount == 0 && amountType != AmountType.None;

            return typeIsNotSpecified || amountIsNotSpecified;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Ingredient;
            yield return Math.Round(Amount, 1);
            yield return AmountType;
        }
    }

    public enum AmountType
    {
        None,
        GRAM,
        KILOGRAM,
        PIECE,
        MILILITER,
        LITER,
        TABLESPOON,
        TEASPOON
    }
}
