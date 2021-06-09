using KitProjects.MasterChef.Dal;
using KitProjects.MasterChef.WebApplication.Extensions;
using KitProjects.MasterChef.WebApplication.Models.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;

namespace WebApplication
{
    public class Startup
    {
        private readonly Container _container = new();

        public void ConfigureServices(IServiceCollection services)
        {
            _container.Options.DefaultLifestyle = Lifestyle.Scoped;
            services.AddMvcCore();
            services.AddCors();
            services.AddSimpleInjector(_container, options =>
            {
                options
                    .AddAspNetCore()
                    .AddControllerActivation();
            });
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer("Server=localhost;Database=MasterChef;Trusted_Connection=True;"));
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API агрегатора кулинарных рецептов \"Мастер Шеф\"", Version = "v1" });
                var xmlDocPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlDocPath);
            });

            _container.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    await context.Response.WriteAsJsonAsync(new ApiErrorResponse(new[] { exceptionHandlerPathFeature.Error.Message }));
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                });
            });

            app.UseSimpleInjector(_container);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = string.Empty;
            });

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
