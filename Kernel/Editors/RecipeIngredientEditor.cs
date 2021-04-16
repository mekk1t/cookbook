using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeIngredientEditor
    {
        private readonly ICommand<EditRecipeIngredientDescriptionCommand> _editIngredientDescription;

        public RecipeIngredientEditor(
            ICommand<EditRecipeIngredientDescriptionCommand> editIngredientDescription)
        {
            _editIngredientDescription = editIngredientDescription;
        }

        public void EditIngredientsDescription(EditRecipeIngredientDescriptionCommand command)
        {
            if (!command.HasValues)
                return;

            _editIngredientDescription.Execute(command);
        }
    }
}
