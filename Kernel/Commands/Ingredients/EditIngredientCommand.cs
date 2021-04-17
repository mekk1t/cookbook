using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class EditIngredientCommand
    {
        public Guid IngredientId { get; }
        public string NewName { get; }

        public EditIngredientCommand(Guid ingredientId, string newName)
        {
            IngredientId = ingredientId;
            NewName = newName;
        }
    }
}
