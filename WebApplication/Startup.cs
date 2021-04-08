using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.Dal.Commands;
using KitProjects.MasterChef.Kernel;
using KitProjects.MasterChef.Kernel.Abstractions;
using KitProjects.MasterChef.Kernel.Models;
using KitProjects.MasterChef.Kernel.Models.Commands;
using KitProjects.MasterChef.Kernel.Models.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer("Server=localhost;Database=MasterChef;Trusted_Connection=True;"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API ���������� ���������� �������� \"������ ���\".", Version = "v1" });
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlDocPath);
            });
            services.AddScoped<CategoryService>();
            services.AddScoped<ICommand<CreateCategoryCommand>, CreateCategoryCommandHandler>();
            services.AddScoped<ICommand<EditCategoryCommand>, EditCategoryCommandHandler>();
            services.AddScoped<ICommand<DeleteCategoryCommand>, DeleteCategoryCommandHandler>();
            services.AddScoped<IQuery<IEnumerable<Category>, GetCategoriesQuery>, GetCategoriesQueryHandler>();

            services.AddScoped<IngredientService>();
            services.AddScoped<ICommand<CreateIngredientCommand>, CreateIngredientCommandHandler>();
            services.AddScoped<ICommand<EditIngredientCommand>, EditIngredientCommandHandler>();
            services.AddScoped<ICommand<DeleteIngredientCommand>, DeleteIngredientCommandHandler>();
            services.AddScoped<IQuery<IEnumerable<Ingredient>, GetIngredientsQuery>, GetIngredientsQueryHandler>();

            services.AddScoped<RecipeService>();
            services.AddScoped<ICommand<CreateRecipeCommand>, CreateRecipeCommandHandler>();
            services.AddScoped<IQuery<IEnumerable<Recipe>, GetRecipesQuery>, GetRecipesQueryHandler>();
            services.AddScoped<ICommand<EditRecipeCommand>, EditRecipeCommandHandler>();
            services.AddScoped<ICommand<DeleteRecipeCommand>, DeleteRecipeCommandHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

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