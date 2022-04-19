namespace KP.Cookbook.Domain.Entities
{
    public class CookingStepsCollection
    {
        private List<CookingStep> _steps;

        public IReadOnlyList<CookingStep> Steps => _steps;

        public CookingStepsCollection(IEnumerable<CookingStep> steps)
        {
            _steps = steps?.ToList() ?? new List<CookingStep>(0);
        }

        public void NormalizeOrder()
        {
            var temp = new List<CookingStep>(_steps.Count);

            for (int i = 0; i < _steps.Count; i++)
            {
                var step = _steps[i];
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
