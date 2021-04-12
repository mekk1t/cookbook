using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitProjects.MasterChef.WebApplication.Recipes.Requests
{
    public class ReplaceIngredientRequest
    {
        public string OldIngredientName { get; set; }
        public string NewIngredientName { get; set; }
    }
}
