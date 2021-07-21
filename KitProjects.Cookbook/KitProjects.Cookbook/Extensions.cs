using KitProjects.Cookbook.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KitProjects.Cookbook
{
    public static class Extensions
    {
        public static void ApplyMigrations(this IApplicationBuilder builder)
        {
            using var dbContext = builder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<CookbookDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
