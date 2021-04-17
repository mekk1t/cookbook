using System;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class RemoveIngredientFromStepCommand
    {
        public RecipeStepIds Ids { get; }
        public Guid IngredientId { get; }

        public RemoveIngredientFromStepCommand(RecipeStepIds ids, Guid ingredientId)
        {
            Ids = ids;
            IngredientId = ingredientId;
        }
    }
}
