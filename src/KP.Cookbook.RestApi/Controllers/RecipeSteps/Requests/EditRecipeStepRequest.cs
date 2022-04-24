namespace KP.Cookbook.RestApi.Controllers.RecipeSteps.Requests
{
    public class EditRecipeStepRequest
    {
        public long StepId { get; set; }
        public string? Description { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
