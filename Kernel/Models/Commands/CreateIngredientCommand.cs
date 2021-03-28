using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class CreateIngredientCommand
    {
        public string Name { get; }
        public IEnumerable<string> Categories { get; }

        public CreateIngredientCommand(string name, IEnumerable<string> categories)
        {
            Name = name;
            Categories = categories;
        }
    }
}
