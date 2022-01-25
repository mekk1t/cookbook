using KP.Cookbook.Domain.ValueObjects;

namespace KP.Cookbook.Domain.Entities
{
    public class Recipe : Entity
    {
        public string Title { get; }
        public RecipeType RecipeType { get; }
        public CookingType CookingType { get; }
        public KitchenType KitchenType { get; }
        public HolidayType HolidayType { get; }
        public DateTime CreatedAt { get; }
        public User Author { get; }
        public List<CookingStep> CookingSteps { get; private set; } = new List<CookingStep>();
        public List<IngredientDetailed> Ingredients { get; } = new List<IngredientDetailed>();

        public Source? Source { get; private set; }
        public int DurationMinutes { get; private set; }
        public string? Description { get; private set; }
        public string? Image { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Recipe(
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
        }

        public void NormalizeSteps()
        {
            var temp = new List<CookingStep>(CookingSteps.Count);

            for (int i = 0; i < CookingSteps.Count; i++)
            {
                var step = CookingSteps[i];
                temp[i] = new CookingStep(step.Id, i + 1)
                {
                    Description = step.Description,
                    Image = step.Image
                };
            }

            CookingSteps.Clear();
            CookingSteps.AddRange(temp);
            temp.Clear();
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
