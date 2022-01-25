using KP.Cookbook.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.Domain.Entities
{
    public class Recipe : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public RecipeType RecipeType { get; set; }
        public CookingType CookingType { get; set; }
        public KitchenType KitchenType { get; set; }
        public HolidayType HolidayType { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User Author { get; set; }
        public Source Source { get; set; }
        public int DurationMinutes { get; set; }
        public List<CookingStep> CookingSteps { get; set; }
        public List<IngredientDetailed> Ingredients { get; set; }
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
