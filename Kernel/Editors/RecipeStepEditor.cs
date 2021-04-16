using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
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
        private readonly IQuery<Recipe, SearchRecipeQuery> _searchRecipe;
        private readonly ICommand<AppendRecipeStepCommand> _appendStep;
        private readonly RecipeIngredientEditor _recipeIngredientEditor;

        public RecipeStepEditor(
            ICommand<EditStepPictureCommand> editPicture,
            ICommand<EditStepDescriptionCommand> editDescription,
            IQuery<RecipeStep, SearchStepQuery> searchStep,
            IQuery<Recipe, SearchRecipeQuery> searchRecipe,
            ICommand<AppendRecipeStepCommand> appendStep,
            RecipeIngredientEditor recipeIngredientEditor)
        {
            _editDescription = editDescription;
            _editPicture = editPicture;
            _searchStep = searchStep;
            _searchRecipe = searchRecipe;
            _appendStep = appendStep;
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
    }
}
