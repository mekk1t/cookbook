using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.WebApplication.Models.Requests.Create.Recipe
{
    public class CreateRecipeIngredientDetails
    {
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string IngredientName { get; set; }
        /// <summary>
        /// Количество ингредиента
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Мера объёма.
        /// </summary>
        public Measures Measure { get; set; }
        /// <summary>
        /// Заметки.
        /// </summary>
        public string Notes { get; set; }
    }
}
