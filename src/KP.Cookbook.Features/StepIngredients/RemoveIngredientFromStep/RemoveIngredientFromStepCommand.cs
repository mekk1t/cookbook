namespace KP.Cookbook.Features.StepIngredients.RemoveIngredientFromStep
{
    public class RemoveIngredientFromStepCommand
    {
        public long StepId { get; }
        public long IngredientId { get; }

        public RemoveIngredientFromStepCommand(long stepId, long ingredientId)
        {
            StepId = stepId;
            IngredientId = ingredientId;
        }
    }
}
