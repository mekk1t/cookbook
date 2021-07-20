using KitProjects.Api.AspNetCore;
using KitProjects.Api.AspNetCore.Extensions;
using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.WebApplication;
using KitProjects.MasterChef.WebApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using System.Reflection;

namespace WebApplication
{
    public class Startup
    {
        private readonly Container _container = new();

        public void ConfigureServices(IServiceCollection services)
        {
            _container.Options.DefaultLifestyle = Lifestyle.Scoped;
            services.AddCors();
            services.AddSimpleInjector(_container, options =>
            {
                options
                    .AddAspNetCore()
                    .AddControllerActivation();
            });
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer("Server=localhost;Database=MasterChef;Trusted_Connection=True;"));

            services.AddApiCore(true, mvc => mvc.Conventions.Add(
                new RouteTokenTransformerConvention(new LowercaseControllerTokenTransformer())));

            services.AddSwaggerV1(
                title: "API агрегатора кулинарных рецептов \"Мастер Шеф\"",
                xmlDocumentationFileName: $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

            _container.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    await context.Response.WriteAsJsonAsync(new ApiErrorResponse(new[] { exceptionHandlerPathFeature.Error.Message }));
                });
            });

            app.UseSimpleInjector(_container);
            app.UseSwaggerDocumentation("Мастер-Шеф: API");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _container.Verify();
        }
    }
}
