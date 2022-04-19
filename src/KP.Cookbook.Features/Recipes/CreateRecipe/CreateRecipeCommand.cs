using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes.CreateRecipe
{
    public class CreateRecipeCommand
    {
        public string Title { get; }
        public RecipeType RecipeType { get; }
        public CookingType CookingType { get; }
        public KitchenType KitchenType { get; }
        public HolidayType HolidayType { get; }
        public string UserLogin { get; }

        public CreateRecipeCommand(
            string title,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            string userLogin)
        {
            Title = title;
            RecipeType = recipeType;
            CookingType = cookingType;
            KitchenType = kitchenType;
            HolidayType = holidayType;
            UserLogin = userLogin;
        }
    }
}
