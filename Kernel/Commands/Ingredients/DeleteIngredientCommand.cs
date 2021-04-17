using System;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class DeleteIngredientCommand
    {
        public Guid IngredientId { get; }

        public DeleteIngredientCommand(Guid ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
