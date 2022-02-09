using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Ingredients.UpdateIngredient
{
    public class UpdateIngredientCommand
    {
        public Ingredient NewIngredient { get; }

        public UpdateIngredientCommand(Ingredient newIngredient)
        {
            NewIngredient = newIngredient;
        }
    }
}
