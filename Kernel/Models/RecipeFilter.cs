namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeFilter
    {
        public string SearchTerm { get; set; }
        public string CategoryName { get; set; }
        public string IngredientCategory { get; set; }
        public int StepsCount { get; set; }
    }
}
