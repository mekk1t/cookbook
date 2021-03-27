using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using System;
using System.Collections.Generic;

namespace KitProjects.MasterChef.Kernel.Services
{
    public class RecipeService
    {
        private readonly IRepository<Recipe, Guid> _recipeRepository;

        public RecipeService(IRepository<Recipe, Guid> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public IEnumerable<Recipe> SearchRecipes(ListQueryFilter filter) => _recipeRepository.Read(filter);
        public Recipe FindRecipe(Guid recipeId) => _recipeRepository.Find(recipeId);
        public void DeleteRecipe(Recipe recipe) => _recipeRepository.Delete(recipe);
        public void EditRecipe(Recipe recipe) => _recipeRepository.Update(recipe);
        public void CreateNewRecipe(Recipe recipe) => _recipeRepository.Create(recipe);
    }
}
