using System;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class RecipeViewModel
    {
        /// <summary>
        /// Идентификатор рецепта в формате GUID.
        /// </summary>
        public Guid RecipeId { get; }
        /// <summary>
        /// Название рецепта.
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Описание рецепта.
        /// </summary>
        public string Description { get; }

        public RecipeViewModel(Guid recipeId, string title, string description)
        {
            RecipeId = recipeId;
            Title = title;
            Description = description;
        }
    }
}
