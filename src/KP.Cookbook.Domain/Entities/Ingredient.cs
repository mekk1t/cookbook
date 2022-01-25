namespace KP.Cookbook.Domain.Entities
{
    /// <summary>
    /// Ингредиент.
    /// </summary>
    public class Ingredient : Entity
    {
        /// <summary>
        /// Название ингредиента.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Тип | категория ингредиента.
        /// </summary>
        public IngredientType Type { get; private set; }

        /// <summary>
        /// Описание ингредиента, если есть.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Создает объект ингредиента.
        /// </summary>
        /// <param name="name">Название ингредиента.</param>
        /// <param name="type">Тип ингредиента.</param>
        /// <exception cref="ArgumentNullException">Пустое название.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Недопустимое значение перечисления <see cref="IngredientType"/>.</exception>
        public Ingredient(string name, IngredientType type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Название ингредиента не может быть пустым.");
            if (Enum.IsDefined(type))
                Type = type;
            else
                throw new ArgumentOutOfRangeException(nameof(type), "Недопустимый тип ингредиента.");
        }
    }

    public enum IngredientType
    {
        /// <summary>
        /// Обычный.
        /// </summary>
        Common,
        /// <summary>
        /// Овощ.
        /// </summary>
        Vegetable,
        /// <summary>
        /// Фрукт.
        /// </summary>
        Fruit,
        /// <summary>
        /// Ягода.
        /// </summary>
        Berry,
        /// <summary>
        /// Гриб.
        /// </summary>
        Mushroom,
        /// <summary>
        /// Зелень.
        /// </summary>
        Greenery,
        /// <summary>
        /// Крупа.
        /// </summary>
        Groats,
        /// <summary>
        /// Кондитерское изделие.
        /// </summary>
        Confectionery,
        /// <summary>
        /// Консервы.
        /// </summary>
        Canned,
        /// <summary>
        /// Кофе.
        /// </summary>
        Coffee,
        /// <summary>
        /// Чай.
        /// </summary>
        Tea,
        /// <summary>
        /// Макаронное изделие.
        /// </summary>
        Pasta,
        /// <summary>
        /// Молочный продукт.
        /// </summary>
        Milk,
        /// <summary>
        /// Кисломолочный продукт.
        /// </summary>
        FermentedMilk,
        /// <summary>
        /// Мука | мучное изделие.
        /// </summary>
        Flour,
        /// <summary>
        /// Мясо.
        /// </summary>
        Meat,
        /// <summary>
        /// Напиток.
        /// </summary>
        Drink,
        /// <summary>
        /// Орех.
        /// </summary>
        Nuts,
        /// <summary>
        /// Семечки.
        /// </summary>
        Seeds,
        /// <summary>
        /// Сухофрукты.
        /// </summary>
        DriedFruit,
        /// <summary>
        /// Пищевая добавка.
        /// </summary>
        Supplement,
        /// <summary>
        /// Специя | приправа.
        /// </summary>
        Spice,
        /// <summary>
        /// Птица.
        /// </summary>
        Bird,
        /// <summary>
        /// Рыба.
        /// </summary>
        Fish,
        /// <summary>
        /// Соус | заправка.
        /// </summary>
        Sauce
    }
}
