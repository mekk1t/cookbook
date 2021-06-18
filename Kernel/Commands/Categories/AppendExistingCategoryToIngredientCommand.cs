using System;

namespace KitProjects.MasterChef.Kernel.Ingredients.Commands
{
    public class AppendExistingCategoryToIngredientCommand
    {
        public string CategoryName { get; }
        public Guid IngredientId { get; }

        public AppendExistingCategoryToIngredientCommand(string categoryName, Guid ingredientId)
        {
            CategoryName = categoryName;
            IngredientId = ingredientId;
        }
    }
}
