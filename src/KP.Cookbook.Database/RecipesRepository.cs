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
                    duration_minutes,
                    description,
                    image,
                    updated_at
                FROM
                    recipes;
            ";

            return _unitOfWork.Execute((c, t) => c.Query<Recipe>(sql, transaction: t).ToList());
        }

        public Recipe GetRecipe(long recipeId) => throw new NotImplementedException("Будет реализовано после поддержки пользователей.");

        public void DeleteById(long recipeId)
        {
            string sql = "DELETE FROM recipes WHERE id = @Id";
            var parameters = new { Id = recipeId };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, transaction: t));
        }

        /// <summary>
        /// Создает рецепт в базе данных.
        /// </summary>
        /// <remarks>
        /// Не создает связанные шаги, пользователя, источник и ингредиенты. Пока.
        /// </remarks>
        /// <param name="recipe"></param>
        public Recipe Create(Recipe recipe)
        {
            string sql = @"
                INSERT INTO recipes
                (
                    title,
                    recipe_type,
                    cooking_type,
                    kitchen_type,
                    holiday_type,
                    created_at,
                    duration_minutes,
                    description,
                    image,
                    updated_at
                )
                VALUES
                (
                    @Title,
                    @RecipeType,
                    @CookingType,
                    @KitchenType,
                    @HolidayType,
                    @CreatedAt,
                    @DurationMin,
                    @Description,
                    @Image,
                    @UpdatedAt
                )
                RETURNING
                    id,
                    title,
                    recipe_type,
                    cooking_type,
                    kitchen_type,
                    holiday_type,
                    created_at,
                    duration_minutes,
                    description,
                    image,
                    updated_at;
            ";

            var parameters = new
            {
                Title = recipe.Title,
                RecipeType = recipe.RecipeType,
                CookingType = recipe.CookingType,
                KitchenType = recipe.KitchenType,
                HolidayType = recipe.HolidayType,
                CreatedAt = recipe.CreatedAt,
                DurationMinutes = recipe.DurationMinutes,
                Description = recipe.Description,
                Image = recipe.Image,
                UpdatedAt = recipe.UpdatedAt
            };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<Recipe>(sql, transaction: t));
        }

        public void Update(Recipe recipe)
        {
            string sql = @"
                UPDATE recipes
                SET
                    title = @Title,
                    recipe_type = @RecipeType,
                    cooking_type = @CookingType,
                    kitchen_type = @KitchenType,
                    holiday_type = @HolidayType,
                    created_at = @CreatedAt,
                    duration_minutes = @DurationMinutes,
                    description = @Description,
                    image = @Image,
                    updated_at = @UpdatedAt
                WHERE
                    id = @Id;
            ";

            var parameters = new
            {
                Id = recipe.Id,
                Title = recipe.Title,
                RecipeType = recipe.RecipeType,
                CookingType = recipe.CookingType,
                KitchenType = recipe.KitchenType,
                HolidayType = recipe.HolidayType,
                CreatedAt = recipe.CreatedAt,
                DurationMinutes = recipe.DurationMinutes,
                Description = recipe.Description,
                Image = recipe.Image,
                UpdatedAt = recipe.UpdatedAt
            };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, transaction: t));
        }
    }
}
