using KitProjects.MasterChef.Kernel.Models.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.Kernel.Models.Steps
{
    public class EditStepIngredientDescriptionCommand
    {
        public Guid RecipeId { get; }
        public Guid StepId { get; }
        public decimal Amount { get; }
        public Measures Measure { get; }
    }
}
