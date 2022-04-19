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
using KP.Cookbook.Features.Sources.UpdateSource;
using KP.Api.AspNetCore.Extensions;

var container = new Container();
container.Options.DefaultLifestyle = Lifestyle.Scoped;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration config = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddHttpContextAccessor();
services.AddSwaggerV1("Cookbook", "KP.Cookbook.RestApi");
services.AddApiCore(true);

services.AddSimpleInjector(container, options => options.AddAspNetCore().AddControllerActivation());

var featuresAssembly = typeof(UpdateSourceCommandHandler).Assembly;

container.Register<IUnitOfWork, UnitOfWork>();
container.Register<Func<DbConnection>>(() => () => new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgresql")));
container.Register(typeof(ICommandHandler<>), featuresAssembly);
container.Register(typeof(ICommandHandler<,>), featuresAssembly);
container.Register(typeof(IQueryHandler<,>), featuresAssembly);
container.RegisterDecorator(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
container.RegisterDecorator(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
container.Register<UnitOfWork>();
container.Register<SourcesRepository>();
container.Register<IngredientsRepository>();
container.Register<UsersRepository>();
container.Register<RecipesRepository>();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

app.Services.UseSimpleInjector(container);

app.UseSwaggerDocumentation("KP: Cookbook");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

container.Verify();

app.Run();
