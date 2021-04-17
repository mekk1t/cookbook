using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Decorators;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Ingredients.Commands;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes;
using KitProjects.MasterChef.Kernel.Recipes.Commands;
using KitProjects.MasterChef.Kernel.Recipes.Commands.Ingredients;
using SimpleInjector;
using System;

namespace KitProjects.MasterChef.WebApplication.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplicationServices(this Container container)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            container.Register(typeof(ICommand<>), assemblies);

            container.RegisterDecorator<ICommand<CreateCategoryCommand>, CreateCategoryDecorator>();
            container.RegisterDecorator<ICommand<CreateIngredientCommand>, CreateIngredientDecorator>();
            container.RegisterDecorator<ICommand<CreateRecipeCommand>, CreateRecipeDecorator>();

            container.RegisterDecorator<ICommand<AppendIngredientCategoryCommand>, AppendCategoryToIngredientDecorator>();
            container.RegisterDecorator<ICommand<RemoveIngredientCategoryCommand>, RemoveCategoryFromIngredientDecorator>();
            container.RegisterDecorator<ICommand<ReplaceRecipeIngredientCommand>, ReplaceIngredientInRecipeDecorator>();
            container.RegisterDecorator<ICommand<ReplaceIngredientsListCommand>, ReplaceIngredientsListInRecipeDecorator>();
            container.RegisterDecorator<ICommand<SwapStepsCommand>, SwapRecipeStepsDecorator>();

            container.RegisterDecorator<ICommand<AppendCategoryToRecipeCommand>, AppendCategoryToRecipeDecorator>();
            container.RegisterDecorator<ICommand<RemoveRecipeCategoryCommand>, RemoveCategoryFromRecipeDecorator>();

            container.RegisterDecorator<ICommand<AppendRecipeIngredientCommand>, AppendIngredientToRecipeDecorator>();
            container.RegisterDecorator<ICommand<EditRecipeIngredientDescriptionCommand>, EditRecipeIngredientDescriptionDecorator>();
            container.RegisterDecorator<ICommand<RemoveRecipeIngredientCommand>, RemoveIngredientFromRecipeDecorator>();

            container.RegisterDecorator<ICommand<AppendRecipeStepCommand>, AppendStepDecorator>();
            container.RegisterDecorator<ICommand<EditStepDescriptionCommand>, EditRecipeStepDescriptionDecorator>();
            container.RegisterDecorator<ICommand<EditStepPictureCommand>, EditRecipeStepPictureDecorator>();
            container.RegisterDecorator<ICommand<RemoveRecipeStepCommand>, RemoveStepFromRecipeDecorator>();

            container.Register(typeof(IQuery<,>), assemblies);
            container.Register(typeof(IQuery<>), assemblies);
            container.Register(typeof(IEntityChecker<,>), assemblies);

            container.Register<CreateRecipeDecorator>();
            container.Register<RemoveCategoryFromIngredientDecorator>();
            container.Register<AppendStepDecorator>();
            container.Register<EditRecipeIngredientDescriptionDecorator>();
        }
    }
}
