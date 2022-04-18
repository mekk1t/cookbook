using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Recipes
{
    public class CreateRecipeCommand
    {
        public string Title { get; }
        public RecipeType RecipeType { get; }
        public CookingType CookingType { get; }
        public KitchenType KitchenType { get; }
        public HolidayType HolidayType { get; }
        public string UserLogin { get; private set; }
        public string UserNickname { get; private set; }

        private CreateRecipeCommand(
            string title,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            string userLogin = "",
            string userNickname = "")
        {
            Title = title;
            RecipeType = recipeType;
            CookingType = cookingType;
            KitchenType = kitchenType;
            HolidayType = holidayType;
            UserLogin = userLogin;
            UserNickname = userNickname;
        }

        public static CreateRecipeCommand CreateWithUserLogin(
            string title,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            string userLogin) => new(title, recipeType, cookingType, kitchenType, holidayType, userLogin);

        public static CreateRecipeCommand CreateWithUserNickname(
            string title,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            string userNickname) => new(title, recipeType, cookingType, kitchenType, holidayType, userNickname: userNickname);
    }
}
