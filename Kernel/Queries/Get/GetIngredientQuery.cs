using System;

namespace KitProjects.MasterChef.Kernel.Models.Queries.Get
{
    public class GetIngredientQuery
    {
        public string IngredientName { get; }
        public Guid IngredientId { get; }

        public GetIngredientQuery(string ingredientName)
        {
            IngredientName = ingredientName;
        }

        public GetIngredientQuery(Guid ingredientId)
        {
            IngredientId = ingredientId;
        }
    }
}
