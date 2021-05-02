using KitProjects.MasterChef.Kernel.Models.Ingredients;

namespace KitProjects.MasterChef.WebApplication.Recipes.Requests
{
    public class EditIngredientDescriptionRequest
    {
        public decimal Amount { get; set; }
        public Measures Measure { get; set; }
        public string Notes { get; set; }
    }
}
