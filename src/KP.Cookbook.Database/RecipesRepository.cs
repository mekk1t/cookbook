using Dapper;
using KP.Cookbook.Database.Models;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class RecipesRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsersRepository _usersRepository;

        public RecipesRepository(IUnitOfWork unitOfWork, UsersRepository usersRepository)
        {
            _unitOfWork = unitOfWork;
            _usersRepository = usersRepository;
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

        public Recipe GetRecipe(long recipeId)
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
                    updated_at,
                    user_id
                FROM
                    recipes
                WHERE id = @Id;
            ";

            var parameters = new { Id = recipeId };

            var recipe = _unitOfWork.Execute((c, t) => c.QueryFirst<DbRecipe>(sql, parameters, transaction: t));
            var user = _usersRepository.GetById(recipe.UserId);

            return Recipe.Recreate(
                recipe.Id,
                recipe.Title,
                user,
                recipe.RecipeType,
                recipe.CookingType,
                recipe.KitchenType,
                recipe.HolidayType,
                recipe.CreatedAt,
                null,
                recipe.DurationMinutes,
                recipe.Description,
                recipe.Image,
                recipe.UpdatedAt);
        }

        public void DeleteById(long recipeId)
        {
            string sql = "DELETE FROM recipes WHERE id = @Id";
            var parameters = new { Id = recipeId };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, transaction: t));
        }

        /// <summary>
        /// Сохраняет рецепт в базе данных.
        /// </summary>
        /// <remarks>
        /// Не создает связанные шаги, пользователя, источник и ингредиенты. Пока.
        /// </remarks>
        /// <param name="recipe"></param>
        /// <returns>
        /// ID созданного рецепта.
        /// </returns>
        public long Save(Recipe recipe)
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
                    updated_at,
                    user_id
                )
                VALUES
                (
                    @Title,
                    @RecipeType,
                    @CookingType,
                    @KitchenType,
                    @HolidayType,
                    @CreatedAt,
                    @DurationMinutes,
                    @Description,
                    @Image,
                    @UpdatedAt,
                    @UserId
                )
                RETURNING
                    id;
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
                UpdatedAt = recipe.UpdatedAt,
                UserId = recipe.Author.Id
            };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<long>(sql, parameters, transaction: t));
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
                    updated_at = @UpdatedAt,
                    source_id = @SourceId
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
                UpdatedAt = recipe.UpdatedAt,
                SourceId = recipe.Source?.Id
            };

            _ = _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, transaction: t));
        }
    }
}
