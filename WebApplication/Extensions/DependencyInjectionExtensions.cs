using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Dal.Commands.Edit.Ingredient;
using KitProjects.MasterChef.Dal.Commands.Edit.Recipe;
using KitProjects.MasterChef.Dal.Commands.Edit.RecipeStep;
using KitProjects.MasterChef.Dal.Queries.Categories;
using KitProjects.MasterChef.Dal.Queries.Ingredients;
using KitProjects.MasterChef.Dal.Queries.Recipes;
using KitProjects.MasterChef.Dal.Queries.Steps;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using KitProjects.MasterChef.Kernel.Models.Queries.Get;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Steps;
using SimpleInjector;
using System.Collections.Generic;

namespace KitProjects.MasterChef.WebApplication.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplicationServices(this Container container)
        {
            container.Register<CategoryService>(Lifestyle.Scoped);
            container.Register<ICommand<CreateCategoryCommand>, CreateCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<EditCategoryCommand>, EditCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<DeleteCategoryCommand>, DeleteCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<IEnumerable<Category>, GetCategoriesQuery>, GetCategoriesQueryHandler>(Lifestyle.Scoped);

            container.Register<IngredientService>(Lifestyle.Scoped);
            container.Register<ICommand<CreateIngredientCommand>, CreateIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<EditIngredientCommand>, EditIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<DeleteIngredientCommand>, DeleteIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<IEnumerable<Ingredient>, GetIngredientsQuery>, GetIngredientsQueryHandler>(Lifestyle.Scoped);

            container.Register<RecipeService>(Lifestyle.Scoped);
            container.Register<ICommand<CreateRecipeCommand>, CreateRecipeCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<IEnumerable<Recipe>, GetRecipesQuery>, GetRecipesQueryHandler>(Lifestyle.Scoped);
            container.Register<ICommand<EditRecipeCommand>, EditRecipeCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<DeleteRecipeCommand>, DeleteRecipeCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<RecipeDetails, GetRecipeQuery>, GetRecipeQueryHandler>(Lifestyle.Scoped);

            container.Register<RecipeEditor>(Lifestyle.Scoped);
            container.Register<ICommand<RemoveRecipeCategoryCommand>, RemoveRecipeCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<AppendCategoryToRecipeCommand>, AppendCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<Category, SearchCategoryQuery>, SearchCategoryQueryHandler>(Lifestyle.Scoped);
            container.Register<IQuery<Recipe, SearchRecipeQuery>, SearchRecipeQueryHandler>(Lifestyle.Scoped);

            container.Register<IngredientEditor>(Lifestyle.Scoped);
            container.Register<ICommand<RemoveIngredientCategoryCommand>, RemoveIngredientCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<AppendIngredientCategoryCommand>, AppendIngredientCategoryCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<Ingredient, SearchIngredientQuery>, SearchIngredientQueryHandler>(Lifestyle.Scoped);

            container.Register<RecipeStepEditor>(Lifestyle.Scoped);
            container.Register<ICommand<EditStepPictureCommand>, EditStepPictureCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<EditStepDescriptionCommand>, EditStepDescriptionCommandHandler>(Lifestyle.Scoped);
            container.Register<IQuery<RecipeStep, SearchStepQuery>, SearchStepQueryHandler>(Lifestyle.Scoped);
            container.Register<ICommand<SwapStepsCommand>, SwapStepsCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<AppendRecipeStepCommand>, AppendRecipeStepCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<RemoveRecipeStepCommand>, RemoveRecipeStepCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<NormalizeStepsOrderCommand>, NormalizeStepsOrderCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<ReplaceStepCommand>, ReplaceStepCommandHandler>(Lifestyle.Scoped);

            container.Register<RecipeIngredientEditor>(Lifestyle.Scoped);
            container.Register<ICommand<AppendRecipeIngredientCommand>, AppendIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<RemoveRecipeIngredientCommand>, RemoveRecipeIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<ReplaceRecipeIngredientCommand>, ReplaceRecipeIngredientCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<ReplaceIngredientsListCommand>, ReplaceRecipeIngredientsListCommandHandler>(Lifestyle.Scoped);
            container.Register<ICommand<EditRecipeIngredientDescriptionCommand>, EditRecipeIngredientDescriptionCommandHandler>(Lifestyle.Scoped);
        }
    }
}
