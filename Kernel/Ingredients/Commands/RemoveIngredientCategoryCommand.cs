using System;

namespace KitProjects.MasterChef.Kernel.Ingredients.Commands
{
    public class RemoveIngredientCategoryCommand
    {
        public string CategoryName { get; }
        public Guid IngredientId { get; }

        public RemoveIngredientCategoryCommand(string categoryName, Guid ingredientId)
        {
            CategoryName = categoryName;
            IngredientId = ingredientId;
        }
    }
}
