using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class CreateRecipeRequest
    {
        /// <summary>
        /// Название рецепта.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Список категорий рецепта.
        /// </summary>
        public IEnumerable<string> Categories { get; set; }
        /// <summary>
        /// Информация об ингредиентах рецепта.
        /// </summary>
        public IEnumerable<RecipeIngredientDetails> IngredientDetails { get; set; }
        /// <summary>
        /// Шаги рецепта.
        /// </summary>
        public IEnumerable<RecipeStep> Steps { get; set; }
    }
}
