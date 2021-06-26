using KitProjects.MasterChef.WebApplication.Models.Requests.Create.Recipe;
using System;
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
        /// Описание рецепта.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Список категорий рецепта.
        /// </summary>
        public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Информация об ингредиентах рецепта.
        /// </summary>
        public IEnumerable<CreateRecipeIngredientDetails> Ingredients { get; set; } = Array.Empty<CreateRecipeIngredientDetails>();
        /// <summary>
        /// Шаги рецепта.
        /// </summary>
        public IEnumerable<CreateRecipeStepDetails> Steps { get; set; } = Array.Empty<CreateRecipeStepDetails>();
    }
}
