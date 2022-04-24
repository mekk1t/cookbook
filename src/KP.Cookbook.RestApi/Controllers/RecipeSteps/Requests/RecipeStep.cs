namespace KP.Cookbook.RestApi.Controllers.RecipeSteps.Requests
{
    public class RecipeStep
    {
        public int Order { get; set; }
        public string? Description { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
