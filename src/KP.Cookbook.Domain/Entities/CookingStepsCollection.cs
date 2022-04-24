namespace KP.Cookbook.Domain.Entities
{
    public class CookingStepsCollection
    {
        private readonly List<CookingStep> _steps;

        public IReadOnlyList<CookingStep> Steps => _steps;
        public bool IsEmpty => _steps.Count == 0;

        public CookingStepsCollection(IEnumerable<CookingStep> steps)
        {
            _steps = steps?.ToList() ?? new List<CookingStep>(0);
        }

        public void NormalizeOrder()
        {
            var temp = new List<CookingStep>(_steps.Count);
            var orderedSteps = _steps.OrderBy(s => s.Order).ToArray();

            for (int i = 0; i < orderedSteps.Length; i++)
            {
                var step = orderedSteps[i];
                temp[i] = new CookingStep(step.Id, i + 1)
                {
                    Description = step.Description,
                    Image = step.Image
                };
            }

            _steps.Clear();
            _steps.AddRange(temp);
            temp.Clear();
        }
    }
}
