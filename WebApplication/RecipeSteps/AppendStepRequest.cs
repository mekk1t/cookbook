using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.RecipeSteps
{
    public class AppendStepRequest
    {
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public IEnumerable<StepIngredientRequestDetails> Ingredients { get; set; }
    }
}
