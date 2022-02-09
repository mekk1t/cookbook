using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Ingredients.CreateIngredient
{
    public class CreateIngredientCommand
    {
        public Ingredient Ingredient { get; }

        public CreateIngredientCommand(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }
    }
}
