using System.Collections.Generic;

namespace KP.Cookbook.RestApi.Controllers.RecipeSteps.Requests
{
    public class AddStepsToRecipeRequest
    {
        public List<RecipeStep> RecipeSteps { get; set; }
    }
}
