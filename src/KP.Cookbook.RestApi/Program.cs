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
using KP.Cookbook.Features.Sources.UpdateSource;
using KP.Cookbook.Features.Abstractions;
using KP.Cookbook.RestApi;
using KitProjects.Api.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var container = new Container();
container.Options.DefaultLifestyle = Lifestyle.Scoped;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration config = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddHttpContextAccessor();
services.AddSwaggerV1("Cookbook", "KP.Cookbook.RestApi");
services.AddApiCore(true);
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
        };
    });

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
container.Register<IAccessValidator, AccessValidator>();

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
