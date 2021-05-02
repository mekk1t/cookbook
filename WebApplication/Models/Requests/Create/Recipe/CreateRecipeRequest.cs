using KitProjects.MasterChef.WebApplication.Models.Requests.Create.Recipe;
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
        public IEnumerable<CreateRecipeIngredientDetails> Ingredients { get; set; }
        /// <summary>
        /// Шаги рецепта.
        /// </summary>
        public IEnumerable<CreateRecipeStepDetails> Steps { get; set; }
    }
}
