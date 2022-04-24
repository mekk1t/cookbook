using Dapper;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class CookingStepsRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CookingStepsRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Создает шаги рецепта и возвращает коллекцию шагов с проставленными ID.
        /// </summary>
        public CookingStepsCollection CreateSteps(IEnumerable<CookingStep> steps)
        {
            string sql = @"
                INSERT INTO cooking_steps
                (
                    ""order"",
                    description,
                    image
                )
                VALUES
                (
                    @Order,
                    @Description,
                    @Image
                )
                RETURNING
                    id,
                    ""order"",
                    description,
                    image;
            ";

            var resultSteps = new List<CookingStep>(steps.Count());

            foreach (var step in steps)
            {
                var parameters = new { Order = step.Order, Description = step.Description, Image = step.Image };

                var resultStep = _unitOfWork.Execute((c, t) => c.QueryFirst<CookingStep>(sql, parameters, t));
                resultSteps.Add(resultStep);
            }

            return new CookingStepsCollection(resultSteps);
        }

        public CookingStep GetById(long stepId)
        {
            string sql = @"
                SELECT id, ""order"", description, image
                FROM cooking_steps
                WHERE id = @StepId;
            ";

            var parameters = new { StepId = stepId };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<CookingStep>(sql, parameters, t));
        }

        public void Update(CookingStep step)
        {
            string sql = @"
                UPDATE cooking_steps
                SET
                    description = @Description,
                    image = @Image
                WHERE
                    id = @StepId;
            ";

            var parameters = new
            {
                Description = step.Description,
                Image = step.Image,
                StepId = step.Id
            };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }
    }
}
