namespace KP.Cookbook.Features.StepIngredients.GetStepIngredients
{
    public class GetStepIngredientsQuery
    {
        public long StepId { get; }

        public GetStepIngredientsQuery(long stepId)
        {
            StepId = stepId;
        }
    }
}
