using KitProjects.Cookbook.Domain.Models;

namespace KitProjects.Cookbook.UI.Models
{
    public class IngredientFormModel
    {
        /// <summary>
        /// Приставка к названию инпутов ингредиента. Является названием свойств, содержащих поля объекта <see cref="IngredientDetails"/>.
        /// </summary>
        /// <remarks>
        /// Например, <c>Recipe.Steps[0]</c> или <c>Recipe</c>.
        /// </remarks>
        public string Prefix { get; set; }
        /// <summary>
        /// Номер ингредиента в содержащем списке ингредиентов.
        /// </summary>
        public int Order { get; set; }
    }
}
