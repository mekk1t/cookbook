﻿using KitProjects.MasterChef.Kernel.Models;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Ingredients
{
    public class GetIngredientsResponse
    {
        /// <summary>
        /// Список ингредиентов.
        /// </summary>
        public IEnumerable<Ingredient> Ingredients { get; }

        public GetIngredientsResponse(IEnumerable<Ingredient> ingredients)
        {
            Ingredients = ingredients;
        }
    }
}
