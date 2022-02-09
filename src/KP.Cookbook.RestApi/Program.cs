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
using KP.Cookbook.Features.Sources.CreateSource;
using KP.Cookbook.Features.Sources.UpdateSource;
using KP.Cookbook.Features.Sources.GetSources;
using System.Linq;

var container = new Container();
container.Options.DefaultLifestyle = Lifestyle.Scoped;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSimpleInjector(container, options => options.AddAspNetCore().AddControllerActivation());

var assemblies = new[] { typeof(UpdateSourceCommandHandler).Assembly };

container.Register<IUnitOfWork, UnitOfWork>();
container.Register<Func<DbConnection>>(() => () => new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgresql")));
container.Register(typeof(ICommandHandler<>), assemblies);
container.Register(typeof(ICommandHandler<,>), assemblies);
container.Register(typeof(IQueryHandler<,>), assemblies);
container.RegisterDecorator(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
container.Register<UnitOfWork>();
container.Register<SourcesRepository>();
container.Register<IngredientsRepository>();

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
