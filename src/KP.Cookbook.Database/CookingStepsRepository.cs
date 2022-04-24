using Dapper;
using KP.Cookbook.Database.Models;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;

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
                    ""order"" = @Order,
                    description = @Description,
                    image = @Image
                WHERE
                    id = @StepId;
            ";

            var parameters = new
            {
                Description = step.Description,
                Image = step.Image,
                StepId = step.Id,
                Order = step.Order
            };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }

        public void Delete(long stepId)
        {
            string sql = "DELETE FROM cooking_steps WHERE id = @StepId";
            var parameters = new { StepId = stepId };
            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }

        public List<IngredientDetailed> GetStepIngredients(long stepId)
        {
            string sql = @"
                SELECT i.id AS ingredient_id, i.name, i.type, i.description, csai.amount, csai.amount_type, csai.is_optional
                FROM ingredients i INNER JOIN cooking_steps_and_ingredients csai ON i.id = csai.ingredient_id
                WHERE csai.cooking_step_id = @StepId;
            ";

            var parameters = new { StepId = stepId };
            var result = _unitOfWork.Execute((c, t) => c.Query<DbIngredientDetailed>(sql, parameters, t));

            return result
                .Select(r => new IngredientDetailed(new Ingredient(r.IngredientId, r.Name, r.Type, r.Description), r.Amount, r.AmountType)
                {
                    IsOptional = r.IsOptional
                })
                .ToList();
        }

        public void RemoveIngredientFromStep(long stepId, long ingredientId)
        {
            string sql = "DELETE FROM cooking_steps_and_ingredients WHERE cooking_step_id = @StepId AND ingredient_id = @IngredientId;";
            var parameters = new { Stepid = stepId, IngredientId = ingredientId };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }
    }
}
