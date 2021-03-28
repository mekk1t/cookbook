using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class CreateIngredientRequest
    {
        public string Name { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
