using System;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class RecipeViewModel
    {
        public Guid RecipeId { get; }
        public string Title { get; }
        public string Description { get; }

        public RecipeViewModel(Guid recipeId, string title, string description)
        {
            RecipeId = recipeId;
            Title = title;
            Description = description;
        }
    }
}
