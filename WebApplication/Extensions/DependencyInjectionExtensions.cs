using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Recipes;
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

            container.Register(typeof(IQuery<,>), assemblies);
            container.Register(typeof(IQuery<>), assemblies);

            container.Register<CreateRecipeDecorator>();
            container.Register<RecipeEditor>();
            container.Register<IngredientEditor>();
            container.Register<RecipeStepEditor>();
            container.Register<RecipeIngredientEditor>();
        }
    }
}
