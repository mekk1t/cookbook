using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Models.Commands
{
    public class CreateRecipeCommand
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public IEnumerable<string> Categories { get; }
        public IEnumerable<RecipeIngredientDetails> IngredientsDetails { get; }

        public CreateRecipeCommand(Guid id, string title,
            IEnumerable<string> categories,
            IEnumerable<RecipeIngredientDetails> ingredientsDetails,
            string description = "")
        {
            Id = id;
            Title = title;
            Description = description;
            Categories = categories;
            IngredientsDetails = ingredientsDetails;
        }
    }
}
