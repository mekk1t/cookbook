namespace KP.Cookbook.Features.RecipeSteps.EditRecipeStep
{
    public class EditRecipeStepCommand
    {
        public long StepId { get; }
        public string? Description { get; }
        public string? Image { get; }

        public EditRecipeStepCommand(long stepId, string? description, string? image)
        {
            StepId = stepId;
            Description = description;
            Image = image;
        }
    }
}
