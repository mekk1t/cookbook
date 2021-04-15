namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class EditRecipeRequest
    {
        /// <summary>
        /// Новое описание рецепта.
        /// </summary>
        public string NewDescription { get; set; }
        /// <summary>
        /// Новое название рецепта.
        /// </summary>
        public string NewTitle { get; set; }
    }
}
