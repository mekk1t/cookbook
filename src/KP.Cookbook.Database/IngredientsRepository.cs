using Dapper;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class IngredientsRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public IngredientsRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Ingredient Create(Ingredient ingredient)
        {
            var sql = @"
                INSERT INTO ingredients
                (
                    name,
                    type,
                    description
                )
                VALUES
                (
                    @Name,
                    @Type,
                    @Description
                )
                RETURNING
                    id,
                    name,
                    type,
                    description;
            ";

            var parameters = new { ingredient.Name, ingredient.Type, ingredient.Description };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<Ingredient>(new CommandDefinition(sql, parameters, t)));
        }

        public List<Ingredient> Get()
        {
            var sql = @"
                SELECT id, name, type, description
                FROM ingredients;
            ";

            return _unitOfWork.Execute((c, t) => c.Query<Ingredient>(sql, transaction: t).ToList());
        }
    }
}
