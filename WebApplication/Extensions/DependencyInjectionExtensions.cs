using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Ingredients;
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
            container.Register(typeof(ICommand<>), assemblies, Lifestyle.Scoped);
            container.Register(typeof(IQuery<,>), assemblies, Lifestyle.Scoped);
            container.Register(typeof(IQuery<>), assemblies, Lifestyle.Scoped);
            container.Register<CategoryService>(Lifestyle.Scoped);
            container.Register<IngredientService>(Lifestyle.Scoped);
            container.Register<RecipeService>(Lifestyle.Scoped);
            container.Register<RecipeEditor>(Lifestyle.Scoped);
            container.Register<IngredientEditor>(Lifestyle.Scoped);
            container.Register<RecipeStepEditor>(Lifestyle.Scoped);
            container.Register<RecipeIngredientEditor>(Lifestyle.Scoped);
        }
    }
}
