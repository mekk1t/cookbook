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

        public void Delete(long ingredientId)
        {
            var sql = @"
                DELETE FROM ingredients
                WHERE id = @Id;
            ";

            _unitOfWork.Execute((c, t) => c.Execute(sql, new { Id = ingredientId }, t));
        }

        public void Update(Ingredient ingredient)
        {
            var sql = @"
                UPDATE 
                    ingredients
                SET
                    name = @Name,
                    type = @Type,
                    description = @Description
                WHERE
                    id = @Id
                RETURNING 
                    id, name, type, description;
            ";

            var parameters = new { ingredient.Id, ingredient.Name, ingredient.Type, ingredient.Description };

            _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }
    }
}
