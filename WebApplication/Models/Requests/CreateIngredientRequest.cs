using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class CreateIngredientRequest
    {
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Список категорий ингредиента. Если не существуют, будут созданы.
        /// </summary>
        public IEnumerable<string> Categories { get; set; }
    }
}
