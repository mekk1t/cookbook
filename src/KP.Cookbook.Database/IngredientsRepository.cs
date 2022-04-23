using Dapper;
using KP.Cookbook.Database.Models;
using KP.Cookbook.Domain.Entities;
using KP.Cookbook.Domain.ValueObjects;

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

        public List<IngredientDetailed> GetRecipeIngredients(long recipeId)
        {
            string sql = @"
                SELECT
                    rai.amount,
                    rai.amount_type,
                    rai.is_optional,
                    i.id AS ingredient_id,
                    i.name,
                    i.type,
                    i.description
                FROM
                    recipes_and_ingredients rai INNER JOIN ingredients i ON rai.ingredient_id = i.id
                WHERE
                    recipe_id = @RecipeId;
            ";

            var parameters = new { RecipeId = recipeId };

            var dbIngredients = _unitOfWork.Execute((c, t) => c.Query<DbIngredientDetailed>(sql, parameters, t));

            return dbIngredients
                .Select(db => new IngredientDetailed(new Ingredient(db.IngredientId, db.Name, db.Type, db.Description), db.Amount, db.AmountType)
                {
                    IsOptional = db.IsOptional
                })
                .ToList();
        }

        public void AddIngredientsToRecipe(long recipeId, IEnumerable<DbIngredientDetailed> ingredients)
        {
            string sql = @"
                INSERT INTO recipes_and_ingredients
                (
                    recipe_id,
                    ingredient_id,
                    amount,
                    amount_type,
                    is_optional
                )
                VALUES
                (
                    @RecipeId,
                    @IngredientId,
                    @Amount,
                    @AmountType,
                    @IsOptional
                );
            ";

            foreach (var ingredient in ingredients)
            {
                var parameters = new
                {
                    RecipeId = recipeId,
                    IngredientId = ingredient.IngredientId,
                    Amount = ingredient.Amount,
                    AmountType = ingredient.AmountType,
                    IsOptional = ingredient.IsOptional
                };

                _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
            }
        }
    }
}
