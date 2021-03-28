using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.WebApplication.Recipes
{
    public class CreateRecipeRequest
    {
        public string Title { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<RecipeIngredientDetails> IngredientDetails { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
    }
}
