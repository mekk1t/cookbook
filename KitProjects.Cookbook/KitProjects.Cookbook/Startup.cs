using KitProjects.Api.AspNetCore.Extensions;
using KitProjects.Cookbook.Core.Abstractions;
using KitProjects.Cookbook.Core.Models;
using KitProjects.Cookbook.Database;
using KitProjects.Cookbook.Database.Crud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KitProjects.Cookbook
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CookbookDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("Database")));
            services.AddApiCore(true);
            services.AddSwaggerV1("Cookbook API", "KitProjects.Cookbook");

            services.AddScoped<ICrud<Category, long>, CategoryCrud>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplyMigrations();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerDocumentation("Cookbook API");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
