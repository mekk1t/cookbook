using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class DeleteIngredientFromStepCommand
    {
        public RecipeStepIds Ids { get; }
        public Guid IngredientId { get; }

        public DeleteIngredientFromStepCommand(RecipeStepIds ids, Guid ingredientId)
        {
            Ids = ids;
            IngredientId = ingredientId;
        }
    }
}
