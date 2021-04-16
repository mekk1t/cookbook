using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries.Search;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using System;
using System.Linq;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeStepEditor
    {
        private readonly ICommand<EditStepPictureCommand> _editPicture;
        private readonly ICommand<EditStepDescriptionCommand> _editDescription;
        private readonly IQuery<RecipeStep, SearchStepQuery> _searchStep;
        private readonly ICommand<SwapStepsCommand> _swapSteps;
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;
        private readonly ICommand<AppendRecipeStepCommand> _appendStep;
        private readonly ICommand<RemoveRecipeStepCommand> _removeStep;
        private readonly ICommand<NormalizeStepsOrderCommand> _normalizeStepsOrder;
        private readonly RecipeIngredientEditor _recipeIngredientEditor;

        public RecipeStepEditor(
            ICommand<EditStepPictureCommand> editPicture,
            ICommand<EditStepDescriptionCommand> editDescription,
            IQuery<RecipeStep, SearchStepQuery> searchStep,
            ICommand<SwapStepsCommand> swapSteps,
            IQuery<Recipe, SearchRecipeQuery> searchRecipe,
            ICommand<AppendRecipeStepCommand> appendStep,
            ICommand<RemoveRecipeStepCommand> removeStep,
            ICommand<NormalizeStepsOrderCommand> normalizeStepsOrder,
            RecipeIngredientEditor recipeIngredientEditor)
        {
            _editDescription = editDescription;
            _editPicture = editPicture;
            _searchStep = searchStep;
            _swapSteps = swapSteps;
            _searchRecipe = searchRecipe;
            _appendStep = appendStep;
            _removeStep = removeStep;
            _normalizeStepsOrder = normalizeStepsOrder;
            _recipeIngredientEditor = recipeIngredientEditor;
        }

        public void ChangePicture(Guid stepId, string newImage)
        {
            var step = _searchStep.Execute(new SearchStepQuery(stepId));
            if (step == null)
                throw new ArgumentException(null, nameof(stepId));

            // imageValidator.Validate(newImage);
            _editPicture.Execute(new EditStepPictureCommand(newImage, stepId));
        }

        public void ChangeDescription(Guid stepId, string newDescription)
        {
            var step = _searchStep.Execute(new SearchStepQuery(stepId));
            if (step == null)
                throw new ArgumentException(null, nameof(stepId));

            _editDescription.Execute(new EditStepDescriptionCommand(newDescription, stepId));
        }

        public void SwapSteps(Guid firstStepId, Guid secondStepId, Guid recipeId)
        {
            var firstStep = _searchStep.Execute(new SearchStepQuery(firstStepId, new SearchStepQueryParameters(recipeId)));
            if (firstStep == null)
                throw new ArgumentException(null, nameof(firstStepId));
            var secondStep = _searchStep.Execute(new SearchStepQuery(secondStepId, new SearchStepQueryParameters(recipeId)));
            if (secondStep == null)
                throw new ArgumentException(null, nameof(secondStepId));

            _swapSteps.Execute(new SwapStepsCommand(firstStepId, secondStepId));
        }

        public void AppendStep(Guid recipeId, RecipeStep step)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            var recipeIngredientNames = recipe.Ingredients.Select(i => i.Name).ToList();
            foreach (var ingredient in step.IngredientsDetails)
            {
                if (!recipeIngredientNames.Contains(ingredient.IngredientName))
                {
                    _recipeIngredientEditor.AppendIngredient(
                        new AppendRecipeIngredientCommand(
                            recipeId,
                            new Ingredient(
                                Guid.NewGuid(),
                                ingredient.IngredientName),
                            new AppendIngredientParameters(ingredient.Amount, ingredient.Measure)));
                }
            }

            _appendStep.Execute(new AppendRecipeStepCommand(recipeId, step));
        }

        public void RemoveStep(Guid recipeId, Guid stepId)
        {
            var recipe = _searchRecipe.Execute(new SearchRecipeQuery(recipeId));
            if (recipe == null)
                throw new ArgumentException(null, nameof(recipeId));

            var recipeStepsIds = recipe.Steps
                .OrderBy(step => step.Index)
                .Select(step => step.Id).ToList();
            var removingStepIndex = recipeStepsIds.IndexOf(stepId);
            if (removingStepIndex != recipe.Steps.Count - 1)
            {
                _removeStep.Execute(new RemoveRecipeStepCommand(recipeId, stepId));
                _normalizeStepsOrder.Execute(new NormalizeStepsOrderCommand(recipeId, removingStepIndex));
                return;
            }

            _removeStep.Execute(new RemoveRecipeStepCommand(recipeId, stepId));
        }
    }
}
