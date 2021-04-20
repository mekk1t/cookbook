using System;

namespace KitProjects.MasterChef.Kernel.Models.Queries.Get
{
    public class GetRecipeQuery
    {
        public Guid RecipeId { get; }

        public GetRecipeQuery(Guid recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
