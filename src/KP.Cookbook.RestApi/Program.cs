using KP.Cookbook.Database;
using KP.Cookbook.RestApi.Uow;
using Npgsql;
using SimpleInjector;
using System.Data.Common;
using KP.Cookbook.Cqrs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var container = new Container();

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSimpleInjector(container, options => options.AddAspNetCore().AddControllerActivation());

var assemblies = AppDomain.CurrentDomain.GetAssemblies();

container.Register<IUnitOfWork, UnitOfWork>();
container.Register(typeof(Func<DbConnection>), () => new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgresql")));
container.Register(typeof(ICommandHandler<>), assemblies);
container.Register(typeof(ICommandHandler<,>), assemblies);
container.RegisterDecorator(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

app.Services.UseSimpleInjector(container);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

container.Verify();

app.Run();
