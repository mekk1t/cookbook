using KitProjects.Cookbook.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace KitProjects.Cookbook
{
    public static class Extensions
    {
        public static void ApplyMigrations(this IApplicationBuilder builder)
        {
            using var dbContext = builder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<CookbookDbContext>();
            dbContext.Database.Migrate();
        }

        public static string GetAssemblyXmlDocumentationPath(this Type type) =>
            Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetAssembly(type).GetName().Name}.xml");
    }
}
