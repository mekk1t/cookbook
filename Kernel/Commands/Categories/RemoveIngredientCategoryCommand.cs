using System;

namespace KitProjects.MasterChef.Kernel.Ingredients.Commands
{
    public class RemoveIngredientCategoryCommand
    {
        public Guid CategoryId { get; }
        public Guid IngredientId { get; }

        public RemoveIngredientCategoryCommand(Guid categoryId, Guid ingredientId)
        {
            CategoryId = categoryId;
            IngredientId = ingredientId;
        }
    }
}
