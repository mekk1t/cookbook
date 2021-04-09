﻿using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using System;

namespace KitProjects.MasterChef.Kernel.Recipes
{
    public class RecipeEditor
    {
        private readonly ICommand<AppendCategoryToRecipeCommand> _appendCategory;
        private readonly IQuery<Category, SearchCategoryCommand> _searchCategory;
        private readonly IQuery<Recipe, SearchRecipeCommand> _searchRecipe;

        public RecipeEditor(
            ICommand<AppendCategoryToRecipeCommand> appendCategory)
        {
            _appendCategory = appendCategory;
        }

        public void AppendCategory(string categoryName, Guid recipeId)
        {
            var existingCategory = _searchCategory.Execute(new SearchCategoryCommand(categoryName));
            if (existingCategory == null)
                throw new ArgumentException("Нельзя добавить несуществующую категорию.");

            var existingRecipe = _searchRecipe.Execute(new SearchRecipeCommand(recipeId));
            if (existingRecipe == null)
                throw new ArgumentException($"Рецепта с ID {recipeId} не существует.");

            _appendCategory.Execute(new AppendCategoryToRecipeCommand(existingCategory.Id, existingRecipe.Id));
        }
    }
}
