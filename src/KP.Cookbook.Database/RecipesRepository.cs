using Dapper;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class RecipesRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecipesRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Recipe> GetRecipes()
        {
            string sql = @"
                SELECT
                    id,
                    title,
                    recipe_type,
                    cooking_type,
                    kitchen_type,
                    holiday_type,
                    created_at,
                    duration_min,
                    description,
                    image,
                    updated_at
                FROM
                    recipes;
            ";

            return _unitOfWork.Execute((c, t) => c.Query<Recipe>(sql, transaction: t).ToList());
        }
    }
}
