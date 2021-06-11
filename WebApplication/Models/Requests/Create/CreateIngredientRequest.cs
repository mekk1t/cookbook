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
        public string[] Categories { get; set; }
    }
}
