using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Steps;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;
using System.Linq;
using KitProjects.MasterChef.WebApplication.RecipeSteps;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Recipes;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class RecipeStepsManager
    {
        private readonly ICommand<EditStepPictureCommand> _editPicture;
        private readonly ICommand<SwapStepsCommand> _swapSteps;
        private readonly ICommand<RemoveRecipeStepCommand> _removeStep;
        private readonly ICommand<ReplaceStepCommand> _replaceStep;
        private readonly ICommand<EditStepDescriptionCommand> _editDescription;
        private readonly ICommand<AppendRecipeStepCommand> _appendStep;

        public RecipeStepsManager(
            ICommand<RemoveRecipeStepCommand> removeStep,
            ICommand<ReplaceStepCommand> replaceStep,
            ICommand<EditStepPictureCommand> editPicture,
            ICommand<SwapStepsCommand> swapSteps,
            ICommand<EditStepDescriptionCommand> editDescription,
            ICommand<AppendRecipeStepCommand> appendStep)
        {
            _editDescription = editDescription;
            _editPicture = editPicture;
            _removeStep = removeStep;
            _replaceStep = replaceStep;
            _swapSteps = swapSteps;
            _appendStep = appendStep;
        }

        public void AddStepToRecipe(Guid recipeId, AppendStepRequest request)
        {
            var step = new RecipeStep
            {
                Description = request.Description,
                Image = request.ImageBase64
            };
            step.IngredientsDetails.AddRange(request.Ingredients
                .Select(ingredient => new StepIngredientDetails
                {
                    IngredientName = ingredient.IngredientName,
                    Measure = ingredient.Measure,
                    Amount = ingredient.Amount
                }));

            _appendStep.Execute(new AppendRecipeStepCommand(recipeId, step));
        }
    }
}
