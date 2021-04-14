using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.RecipeSteps
{
    public class ReplaceStepRequest
    {
        public string Description { get; set; }
        public string Image { get; set; }
        public List<StepIngredientRequestDetails> Ingredients { get; set; }
    }
}
