using System;

namespace KitProjects.MasterChef.Kernel.Ingredients.Commands
{
    public class AppendIngredientCategoryCommand
    {
        public string CategoryName { get; }
        public Guid IngredientId { get; }

        public AppendIngredientCategoryCommand(string categoryName, Guid ingredientId)
        {
            CategoryName = categoryName;
            IngredientId = ingredientId;
        }
    }
}
