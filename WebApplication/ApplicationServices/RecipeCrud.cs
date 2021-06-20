using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Recipes;
using System.Collections.Generic;
using KitProjects.MasterChef.WebApplication.Models.Filters;
using System;

namespace KitProjects.MasterChef.WebApplication.ApplicationServices
{
    public class RecipeCrud
    {
        private readonly ICommand<CreateRecipeCommand> _createRecipe;
        private readonly IQuery<IEnumerable<Recipe>, GetRecipesQuery> _getRecipes;
        private readonly IQuery<RecipeDetails, GetRecipeQuery> _getRecipe;
        private readonly ICommand<EditRecipeCommand> _editRecipe;
        private readonly ICommand<DeleteRecipeCommand> _deleteRecipe;

        public RecipeCrud(
            ICommand<CreateRecipeCommand> createRecipe,
            IQuery<IEnumerable<Recipe>, GetRecipesQuery> getRecipes,
            IQuery<RecipeDetails, GetRecipeQuery> getRecipe,
            ICommand<EditRecipeCommand> editRecipe,
            ICommand<DeleteRecipeCommand> deleteRecipe)
        {
            _createRecipe = createRecipe;
            _getRecipes = getRecipes;
            _getRecipe = getRecipe;
            _editRecipe = editRecipe;
            _deleteRecipe = deleteRecipe;
        }

        public void Create(CreateRecipeCommand command) => _createRecipe.Execute(command);

        public IEnumerable<Recipe> Read(PaginationFilter pagination) =>
            _getRecipes.Execute(new GetRecipesQuery(pagination.WithRelationships, pagination.Limit, pagination.Offset));

        public RecipeDetails Read(Guid recipeId) => _getRecipe.Execute(new GetRecipeQuery(recipeId));

        public void Update(Guid recipeId, string title, string description) =>
            _editRecipe.Execute(new EditRecipeCommand(recipeId, title, description));

        public void Delete(Guid recipeId) => _deleteRecipe.Execute(new DeleteRecipeCommand(recipeId));
    }
}
