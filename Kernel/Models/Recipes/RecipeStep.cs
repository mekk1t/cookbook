﻿using KitProjects.MasterChef.Kernel.Models.Recipes;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class RecipeStep : Entity
    {
        public RecipeStep(Guid id) : base(id)
        {
            IngredientsDetails = new List<StepIngredientDetails>();
        }

        public int Index { get; init; }
        public string Description { get; init; }
        public string Image { get; init; }
        public List<StepIngredientDetails> IngredientsDetails { get; }
    }
}