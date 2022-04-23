namespace KP.Cookbook.Domain.Entities
{
    public class Recipe : Entity
    {
        public string Title { get; private set; }
        public RecipeType RecipeType { get; private set; }
        public CookingType CookingType { get; private set; }
        public KitchenType KitchenType { get; private set; }
        public HolidayType HolidayType { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User Author { get; }

        public Source? Source { get; private set; }
        public int DurationMinutes { get; private set; }
        public string? Description { get; private set; }
        public string? Image { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Recipe()
        {
        }

        private Recipe(
            long id,
            string title,
            User author,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            Source? source,
            int durationMinutes,
            string? description,
            string? image,
            DateTime? updatedAt) : base(id)
        {
            Title = title;
            Author = author;
            RecipeType = recipeType;
            CookingType = cookingType;
            KitchenType = kitchenType;
            HolidayType = holidayType;
            Source = source;
            DurationMinutes = durationMinutes;
            Description = description;
            Image = image;
            UpdatedAt = updatedAt;
        }

        private Recipe(
            string title,
            User author,
            RecipeType recipeType = RecipeType.Common,
            CookingType cookingType = CookingType.None,
            KitchenType kitchenType = KitchenType.None,
            HolidayType holidayType = HolidayType.None)
        {
            Title = title ?? throw new InvariantException("Не указано название рецепта");
            Author = author ?? throw new InvariantException("Не указан автор рецепта");
            RecipeType = recipeType;
            CookingType = cookingType;
            KitchenType = kitchenType;
            HolidayType = holidayType;
            CreatedAt = DateTime.UtcNow;
        }

        public void Edit(Source? source = null, int durationMinutes = 0, string? description = null, string? image = null)
        {
            if (source != null)
                Source = source;
            if (durationMinutes != default)
                DurationMinutes = durationMinutes;
            if (description != default)
                Description = description;
            if (image != default)
                Image = image;

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Воссоздаёт объект рецепта на основе имеющихся данных.
        /// </summary>
        public static Recipe Recreate(
            long id,
            string title,
            User author,
            RecipeType recipeType,
            CookingType cookingType,
            KitchenType kitchenType,
            HolidayType holidayType,
            Source? source,
            int durationMinutes,
            string? description,
            string? image,
            DateTime? updatedAt) => new(id, title, author, recipeType, cookingType, kitchenType, holidayType, source, durationMinutes, description, image, updatedAt);

        public static Recipe Create(
            string title,
            User author,
            RecipeType recipeType = RecipeType.Common,
            CookingType cookingType = CookingType.None,
            KitchenType kitchenType = KitchenType.None,
            HolidayType holidayType = HolidayType.None) => new(title, author, recipeType, cookingType, kitchenType, holidayType);
    }

    public enum RecipeType
    {
        Common,
        Bakery,
        Soup,
        Vtoroe,
        Dessert,
        Salad,
        Casserole,
        Snack,
        Pancake,
        Fritter,
        Cheesecake
    }

    public enum CookingType
    {
        None,
        Whip,
        Freeze,
        Bake,
        Boil,
        Stew,
        Slice,
        Fry,
        Grill
    }

    public enum KitchenType
    {
        None,
        Russian,
        Armenian,
        Home
    }

    public enum HolidayType
    {
        None,
        NewYear,
        Maslenitsa,
        Easter,
        Birthday
    }
}
